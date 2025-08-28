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

            var playerUnits = Game.Instance.State.Units
                .Where(u => u != null && u.IsInCombat && u.IsPlayerFaction)
                .ToList();

            var enemies = Game.Instance.State.Units
                .Where(u => u != null && u.IsInCombat && !u.IsPlayerFaction)
                .ToList();

            if (enemies.Count == 0)
                return;

            int playerCount = playerUnits.Count;
            int totalPlayerCR = playerUnits.Sum(u => u.Descriptor.Progression.CharacterLevel); 

            float averageCR = playerCount > 0 ? (float)totalPlayerCR / playerCount : 0f; 
            int roundedAverageCR = Mathf.CeilToInt(averageCR) + Config.Settings.ModSettings.Instance.EncounterDifficultyModifier; 
            int adjustedPlayerCR = Mathf.CeilToInt(totalPlayerCR * (1 + Config.Settings.ModSettings.Instance.PartyDifficultyOffset)); 

            int enemyCR = enemies.Sum(u => u.Blueprint.CR); 
            int crDifference = adjustedPlayerCR - enemyCR;  

            int reinforcementsToSpawn = 0;
            if (averageCR > 0f && crDifference > 0)
                reinforcementsToSpawn = Mathf.CeilToInt(crDifference / averageCR); 

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
