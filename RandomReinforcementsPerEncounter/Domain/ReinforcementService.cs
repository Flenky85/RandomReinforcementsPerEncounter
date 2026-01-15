using Kingmaker;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using System.Linq;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.Domain
{
    // Schedules reinforcements based on party vs enemy CR: compute gap, queue N spawns near enemies, then try to spawn.
    internal static class ReinforcementService
    {
        internal static void ScheduleFromCurrentCombat()
        {
            if (State.CombatFlags.ReinforcementsSpawned) return; // idempotent: run once per combat

            State.LootContext.Reset();
            State.CombatFlags.ReinforcementsSpawned = true;

            EncounterReinforcementCalc.Compute(out var enemies, out var roundedAverageCR, out var reinforcementsToSpawn);

            if (enemies.Count == 0)
                return;


            for (int i = 0; i < reinforcementsToSpawn; i++)
            {
                var enemy = enemies[i % enemies.Count]; 
                var position = enemy.Position;

                State.LootContext.ChestPosition = position; 

                string factionId = enemy.Blueprint.m_Faction?.Guid.ToString() ?? BlueprintGuids.Unknown; 
                State.ReinforcementState.Pending.Add((position, roundedAverageCR, factionId)); 
            }

            State.ReinforcementState.TrySpawnPendingReinforcements();
        }
    }
}
