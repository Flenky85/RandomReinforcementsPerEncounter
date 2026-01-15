using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.Domain
{
    internal static class EncounterReinforcementCalc
    {
        internal static void Compute(out List<UnitEntityData> enemies, out int roundedAverageCR, out int reinforcementsToSpawn)
        {
            var playerUnits = Game.Instance.Player.Party
                .Where(u => u != null && u.IsInCombat)
                .ToList();

            enemies = Game.Instance.State.Units
                .Where(u => u != null && u.IsInCombat && !u.IsPlayerFaction)
                .ToList();

            if (enemies.Count == 0)
            {
                roundedAverageCR = 0;
                reinforcementsToSpawn = 0;
                return;
            }

            int playerCount = playerUnits.Count;
            int totalPlayerCR = playerUnits.Sum(u => u.Descriptor.Progression.CharacterLevel);

            float averageCR = playerCount > 0 ? (float)totalPlayerCR / playerCount : 0;
            roundedAverageCR = Mathf.CeilToInt(averageCR) + Config.Settings.ModSettings.Instance.EncounterDifficultyModifier;
            int adjustedPlayerCR = Mathf.CeilToInt(totalPlayerCR * (1 + Config.Settings.ModSettings.Instance.PartyDifficultyOffset));

            int enemyCR = enemies.Sum(u => u.Blueprint.CR);
            int crDifference = adjustedPlayerCR - enemyCR;

            reinforcementsToSpawn = 0;
            if (averageCR > 0f && crDifference > 0)
                reinforcementsToSpawn = Mathf.CeilToInt(crDifference / averageCR);
        }
    }
}
