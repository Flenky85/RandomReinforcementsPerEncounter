using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TurnBased.Controllers;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    /// <summary>
    /// Patch that triggers at the start of combat to evaluate CRs and schedule reinforcements.
    /// </summary>
    [HarmonyPatch(typeof(CombatController), "HandleCombatStart")]
    public static class Patch_CombatStart
    {
        public static void Postfix()
        {
            var playerUnits = Game.Instance.State.Units
                .Where(u => u != null && u.IsInCombat && u.IsPlayerFaction)
                .ToList();

            var enemies = Game.Instance.State.Units
                .Where(u => u != null && u.IsInCombat && !u.IsPlayerFaction)
                .ToList();

            int playerCount = playerUnits.Count;
            int totalPlayerCR = playerUnits.Sum(u => u.Descriptor.Progression.CharacterLevel);

            float averageCR = playerCount > 0 ? (float)totalPlayerCR / playerCount : 0;
            int roundedAverageCR = Mathf.CeilToInt(averageCR) + ModSettings.Instance.EncounterDifficultyModifier;
            int adjustedPlayerCR = Mathf.CeilToInt(totalPlayerCR * (1 + ModSettings.Instance.PartyDifficultyOffset));

            int enemyCR = enemies.Sum(u => u.Blueprint.CR);
            int crDifference = adjustedPlayerCR - enemyCR;
            int reinforcementsToSpawn = averageCR > 0 ? Mathf.CeilToInt(crDifference / averageCR) : 0;

            for (int i = 0; i < reinforcementsToSpawn; i++)
            {
                var enemy = enemies[i % enemies.Count];
                var position = enemy.Position;

                if (ChestSpawn.StoredPosition == null)
                {
                    ChestSpawn.StoredPosition = position;
                    Debug.Log($"[Cloner] 📦 Posición guardada: {position}");
                }

                string factionId = GetBlueprintFactionId(enemy.Blueprint);
                ReinforcementState.Pending.Add((position, roundedAverageCR, factionId));
            }

            ReinforcementState.TrySpawnPendingReinforcements();
        }

        private static string GetBlueprintFactionId(BlueprintUnit blueprint)
        {
            var field = blueprint.GetType().GetField("m_Faction", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) return "UNKNOWN";

            var factionObj = field.GetValue(blueprint);
            var guidProp = factionObj?.GetType().GetProperty("Guid");
            var guid = guidProp?.GetValue(factionObj);

            return guid?.ToString() ?? "UNKNOWN";
        }
    }

    public static class ReinforcementState
    {
        public static readonly List<(Vector3 Position, int CR, string FactionId)> Pending =
            new List<(Vector3 Position, int CR, string FactionId)>();

        public static bool HasPending => Pending.Count > 0;

        public static void TrySpawnPendingReinforcements()
        {
            if (!HasPending) return;

            foreach (var r in Pending)
                ReinforcementUtils.SpawnReinforcementAt(r.Position, r.CR, r.FactionId);

            Pending.Clear();
        }
    }

    public static class ReinforcementUtils
    {
        private static readonly System.Random _rng = new System.Random();

        public static void SpawnReinforcementAt(Vector3 position, int cr, string factionId)
        {
            var assetId = GetRandomAssetIdByCRAndFaction(cr, factionId);
            if (string.IsNullOrEmpty(assetId))
            {
                Debug.LogWarning($"[Reinforcements] No asset found for faction {factionId} at CR {cr}");
                return;
            }

            var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(assetId);
            if (blueprint == null)
            {
                Debug.LogWarning($"[Reinforcements] ❌ Blueprint not found: {assetId}");
                return;
            }

            Debug.Log($"[Reinforcements] 🧪 Using blueprint: {blueprint.name} ({blueprint.AssetGuid})");

            var clone = ManualBlueprintCloner.CloneMinimal(blueprint);
            if (clone == null)
            {
                Debug.LogWarning("[Reinforcements] ❌ Failed to clone blueprint.");
                return;
            }

            Debug.Log($"[Reinforcements] 🧪 Cloned blueprint: {clone.name} ({clone.AssetGuid})");

            Vector3 spawnPos = FindValidPositionNear(position);
            var spawned = Game.Instance.EntityCreator.SpawnUnit(
                clone,
                position,
                Quaternion.identity,
                Game.Instance.State.LoadedAreaState.MainState
            );

            if (spawned == null || spawned.View == null)
            {
                Debug.LogWarning($"[Reinforcements] ❌ Spawn failed for: {assetId}");
                return;
            }

            ConfigureSpawnedUnit(spawned);
            MoveSpawnedToPosition(spawned, spawnPos);
            spawned.CombatState.JoinCombat(true);
        }

        private static string GetRandomAssetIdByCRAndFaction(int cr, string factionId)
        {
            var list = GetMonsterListByFaction(factionId);
            if (list == null)
            {
                Debug.LogWarning($"[Reinforcements] Unknown faction: {factionId}");
                return null;
            }

            while (cr >= 0)
            {
                int searchCR = ApplyCRVariability(cr);
                searchCR = Mathf.Max(1, searchCR);

                var filtered = list.FindAll(m => m.CR == searchCR.ToString());
                if (filtered.Count > 0)
                {
                    var chosen = filtered[_rng.Next(filtered.Count)];
                    Debug.Log($"[Reinforcements] Chosen CR: {searchCR}, Faction: {factionId}, AssetId: {chosen.AssetId}");
                    return chosen.AssetId;
                }

                cr--;
            }

            Debug.LogWarning($"[Reinforcements] No valid monster found for faction {factionId}");
            return null;
        }

        private static List<MonsterData> GetMonsterListByFaction(string factionId)
        {
            return factionId switch
            {
                "28460a5d00a62b742b80c90c37559644" => BanditList.Monsters,
                "0f539babafb47fe4586b719d02aff7c4" => MobList.Monsters,
                "24a215bb66e34153b4d648829c088ae6" => OozeList.Monsters,
                "b1525b4b33efe0241b4cbf28486cd2cc" => WildAnimalsList.Monsters,
                _ => null
            };
        }

        private static int ApplyCRVariability(int baseCR)
        {
            int variability = ModSettings.Instance.VariabilityRange;
            int mode = ModSettings.Instance.VariabilityMode;

            if (variability <= 0) return baseCR;

            return mode switch
            {
                0 => baseCR + _rng.Next(-variability, variability + 1),
                1 => baseCR - _rng.Next(0, variability + 1),
                2 => baseCR + _rng.Next(0, variability + 1),
                _ => baseCR
            };
        }

        private static Vector3 FindValidPositionNear(Vector3 origin)
        {
            float offsetX = (float)(_rng.NextDouble() * 4.0 - 2.0);
            float offsetZ = (float)(_rng.NextDouble() * 4.0 - 2.0);
            return origin + new Vector3(offsetX, 0f, offsetZ);
        }

        private static void ConfigureSpawnedUnit(UnitEntityData unit)
        {
            unit.GiveExperienceOnDeath = false;
            unit.Descriptor.State.AddCondition(UnitCondition.Unlootable);
            Debug.Log("[Cloner] 💀 Unlootable y sin experiencia.");
        }

        private static void MoveSpawnedToPosition(UnitEntityData unit, Vector3 position)
        {
            unit.Position = position;

            if (unit.View != null)
            {
                unit.View.transform.position = position;
                unit.View.transform.rotation = Quaternion.identity;
            }
        }
    }
}
