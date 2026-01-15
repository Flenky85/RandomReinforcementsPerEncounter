using HarmonyLib;
using Kingmaker;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.State;
using System.Linq;
using TurnBased.Controllers;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.Patches
{

    [HarmonyPatch(typeof(CombatController), "HandleCombatStart")]
    public static class CombatStartPatch
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

            Domain.EncounterReinforcementCalc.Compute(out var enemies, out var roundedAverageCR, out var reinforcementsToSpawn);

            if (enemies.Count == 0)
                return;

            for (int i = 0; i < reinforcementsToSpawn; i++)
            {
                var enemy = enemies[i % enemies.Count];
                var position = enemy.Position;

                LootContext.ChestPosition = position;

                string factionId = enemy.Blueprint.m_Faction?.Guid.ToString() ?? BlueprintGuids.Unknown;
                ReinforcementState.Pending.Add((position, roundedAverageCR, factionId));
            }

            ReinforcementState.TrySpawnPendingReinforcements();
        }

    }
}
