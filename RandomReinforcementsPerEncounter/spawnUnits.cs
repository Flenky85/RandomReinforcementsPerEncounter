using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System.Collections.Generic;
using System.Linq;
using TurnBased.Controllers;
using UnityEngine;
using System.Threading.Tasks;


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
            Run();
        }
        public static void Run()
        {
            if (CombatFlags.ReinforcementsSpawned) return;
            
            LootContext.EnemyCRs?.Clear();
            CombatFlags.ReinforcementsSpawned = true;

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

                LootContext.ChestPosition = position;

                string factionId = enemy.Blueprint.m_Faction?.Guid.ToString() ?? "UNKNOWN";
                ReinforcementState.Pending.Add((position, roundedAverageCR, factionId));
            }

            ReinforcementState.TrySpawnPendingReinforcements();
        }

    }

    /// <summary>
    /// Handler that detects combat start and end, also in real-time mode.
    /// EXPERIMENTAL: Works in testing, but may require adjustments if edge cases appear with units joining/leaving combat.
    /// </summary>
    public sealed class MainJoinCombatHandler : IUnitCombatHandler, IGlobalSubscriber
    {
        public static void Init()
        {
            EventBus.Subscribe(new MainJoinCombatHandler());
        }

        public async void HandleUnitJoinCombat(UnitEntityData unit)
        {
            if (Game.Instance?.Player?.IsTurnBasedModeOn() == true)
            {
                UnityEngine.Debug.Log("Skip spawn: TB combat is active");
                return;
            }

            var mainRef = Game.Instance?.Player?.MainCharacter;
            var main = mainRef != null ? mainRef.Value : null;
            if (unit == null || main == null) return;

            if (unit == main)
            {
                await Task.Delay(500);
                Patch_CombatStart.Run();
            }
        }

        public void HandleUnitLeaveCombat(UnitEntityData unit)
        {
            if (Game.Instance?.Player?.IsTurnBasedModeOn() == true)
            {
                UnityEngine.Debug.Log("Skip chest: TB combat is active");
                return;
            }

            bool anyInCombat = Game.Instance.State.Units.Any(u => u != null && u.IsInCombat);
            if (anyInCombat) return;
            ChestSpawn.SpawnLootChest("1ccbdc2361534a8d99e4043b8b345e72", LootContext.ChestPosition.Value);
            CombatFlags.ReinforcementsSpawned = false;
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
                return;
            }
            
            var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(assetId);
            if (blueprint == null)
            {
                return;
            }

            Vector3 spawnPos = FindValidPositionNear(position);
            var spawned = Game.Instance.EntityCreator.SpawnUnit(
                blueprint,
                position,
                Quaternion.identity,
                Game.Instance.State.LoadedAreaState.MainState
            );

            if (spawned == null || spawned.View == null)
            {
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
                    LootContext.EnemyCRs.Add(searchCR);
                    return chosen.AssetId;
                }

                cr--;
            }
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
            unit.View?.gameObject?.AddComponent<CloneMarker>(); // Visual marker used for detection in CloneDeathWatcher
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

    public static class LootContext
    {
        public static Vector3? ChestPosition;
        public static List<int> EnemyCRs = new List<int>(); // Guarda la CR de cada enemigo spawneado
    }

    public static class CombatFlags
    {
        public static bool ReinforcementsSpawned = false; // Protection to ensure reinforcements are only spawned once per combat
    }

}
