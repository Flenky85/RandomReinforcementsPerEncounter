using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using RandomReinforcementsPerEncounter.Config;
using RandomReinforcementsPerEncounter.State;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.GameApi
{
    // Sigue siendo public si lo usas desde fuera; si no, cámbialo a internal.
    public static class ReinforcementSpawner
    {
        private static readonly System.Random _rng = new System.Random();

        public static void SpawnReinforcementAt(Vector3 position, int cr, string factionId)
        {
            var assetId = GetRandomAssetIdByCRAndFaction(cr, factionId);
            if (string.IsNullOrEmpty(assetId)) return;

            var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(assetId);
            if (blueprint == null) return;

            // 1) Spawn en el ANCLA (enemigo)...
            var spawned = Game.Instance.EntityCreator.SpawnUnit(
                blueprint,
                position,
                Quaternion.identity,
                Game.Instance.State.LoadedAreaState.MainState
            );
            if (spawned == null || spawned.View == null) return;

            // 2) ...y luego lo movemos un poco (offset corto, [-2, 2] en X/Z), como en tu versión original.
            Vector3 spawnPos = FindValidPositionNear(position);
            ConfigureSpawnedUnit(spawned);
            MoveSpawnedToPosition(spawned, spawnPos);
            spawned.CombatState.JoinCombat(true);
        }

        private static string GetRandomAssetIdByCRAndFaction(int cr, string factionId)
        {
            var list = GetMonsterListByFaction(factionId);
            if (list == null) return null;

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
                FactionIds.Bandits => BanditList.Monsters,
                FactionIds.Mob => MobList.Monsters,
                FactionIds.Ooze => OozeList.Monsters,
                FactionIds.WildAnimals => WildAnimalsList.Monsters,
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

        // MISMO comportamiento que tenías: desplazamiento pequeño alrededor del ancla
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
            unit.View?.gameObject?.AddComponent<CloneMarker>();
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
