using HarmonyLib;
using Kingmaker;
using RandomReinforcementsPerEncounter.Config;
using RandomReinforcementsPerEncounter.Config.Ids;
using RandomReinforcementsPerEncounter.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.Patches
{
    /// <summary>
    /// Patch that triggers at the start of combat to evaluate CRs and schedule reinforcements.
    /// </summary>
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

            float averageCR = playerCount > 0 ? (float)totalPlayerCR / playerCount : 0;
            int roundedAverageCR = Mathf.CeilToInt(averageCR) + ModSettings.Instance.EncounterDifficultyModifier;
            int adjustedPlayerCR = Mathf.CeilToInt(totalPlayerCR * (1 + ModSettings.Instance.PartyDifficultyOffset));

            int enemyCR = enemies.Sum(u => u.Blueprint.CR);
            int crDifference = adjustedPlayerCR - enemyCR;
            int reinforcementsToSpawn = 0;
            if (averageCR > 0f && crDifference > 0)
                reinforcementsToSpawn = Mathf.CeilToInt(crDifference / averageCR);

            for (int i = 0; i < reinforcementsToSpawn; i++)
            {
                var enemy = enemies[i % enemies.Count];
                var position = enemy.Position;

                LootContext.ChestPosition = position;

                string factionId = enemy.Blueprint.m_Faction?.Guid.ToString() ?? BlueprintGuids.FactionIds.Unknown;
                ReinforcementState.Pending.Add((position, roundedAverageCR, factionId));
            }

            ReinforcementState.TrySpawnPendingReinforcements();
        }

    }
}
