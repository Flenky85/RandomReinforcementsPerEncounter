using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using System.Collections.Generic;
using TurnBased.Controllers;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    /// <summary>
    /// Patch that triggers at the start of combat to evaluate the player's and enemies' CR
    /// and dynamically spawn enemy reinforcements to balance the encounter difficulty.
    /// </summary>
    [HarmonyPatch(typeof(CombatController), "HandleCombatStart")]
    public static class Patch_CombatStart
    {
        public static void Postfix()
        {
            int playerCR = 0;
            int playerCount = 0;

            // Calculate the total level and count of player characters involved in combat.
            foreach (var unit in Game.Instance.State.Units)
            {
                if (unit == null || !unit.IsInCombat) continue;

                if (unit.IsPlayerFaction)
                {
                    playerCR += unit.Descriptor.Progression.CharacterLevel;
                    playerCount++;
                }
            }

            float averageCR = playerCount > 0 ? (float)playerCR / playerCount : 0;
            int roundedAverageCR = Mathf.CeilToInt(averageCR);

            // Calculate the total CR of enemy units currently engaged in combat.
            int enemyCR = 0;
            foreach (var unit in Game.Instance.State.Units)
            {
                if (unit == null || !unit.IsInCombat || unit.IsPlayerFaction) continue;

                enemyCR += unit.Blueprint.CR;
            }

            // Determine how many reinforcements should be spawned based on CR difference.
            int crDifference = playerCR - enemyCR;
            int reinforcementsToSpawn = Mathf.CeilToInt((float)crDifference / averageCR);

            // Collect valid enemy units to use as spawn reference points for reinforcements.
            List<UnitEntityData> enemies = new List<UnitEntityData>();
            foreach (var unit in Game.Instance.State.Units)
            {
                if (unit != null && !unit.IsPlayerFaction && unit.IsInCombat)
                {
                    enemies.Add(unit);
                }
            }

            // Distribute reinforcements around existing enemies, rotating through positions.
            for (int i = 0; i < reinforcementsToSpawn; i++)
            {
                var enemy = enemies[i % enemies.Count];
                var position = enemy.Position;
                string factionId = "UNKNOWN";

                // Attempt to retrieve the internal faction ID from the enemy blueprint.
                var factionField = enemy.Blueprint.GetType().GetField("m_Faction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (factionField != null)
                {
                    var factionValue = factionField.GetValue(enemy.Blueprint);
                
                    var guidProperty = factionValue?.GetType().GetProperty("Guid");
                    if (guidProperty != null)
                    {
                        var guidValue = guidProperty.GetValue(factionValue);
                        factionId = guidValue?.ToString() ?? "UNKNOWN";
                    }
                }

                // Add pending reinforcement to the global spawn queue
                ReinforcementState.Pending.Add((position, roundedAverageCR, factionId));
            }

            // Trigger the actual spawning of pending reinforcements.
            ReinforcementState.TrySpawnPendingReinforcements();
        }
    }

    /// <summary>
    /// Holds reinforcements scheduled for spawning and executes them when ready.
    /// </summary>
    public static class ReinforcementState
    {
        public static List<(Vector3 Position, int CR, string FactionId)> Pending = new List<(Vector3, int, string)>();

        /// <summary>
        /// Spawns all pending reinforcements and clears the queue.
        /// </summary>
        public static void TrySpawnPendingReinforcements()
        {
            if (Pending.Count > 0)
            {
                foreach (var reinforcement in Pending)
                {
                    ReinforcementUtils.SpawnReinforcementAt(reinforcement.Position, reinforcement.CR, reinforcement.FactionId);
                }
                Pending.Clear();
            }
        }
    }

    /// <summary>
    /// Utility class responsible for selecting and spawning reinforcement units.
    /// </summary>
    public static class ReinforcementUtils
    {
        private static readonly System.Random _rng = new System.Random();

        /// <summary>
        /// Spawns a reinforcement unit at a given position, using the requested CR and faction.
        /// </summary>
        public static void SpawnReinforcementAt(Vector3 position, int cr, string factionId)
        {
            var chosenAssetId = GetRandomAssetIdByCRAndFaction(cr, factionId);

            var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(chosenAssetId);
            if (blueprint == null)
            {
                Debug.LogWarning($"[Reinforcements] Blueprint not found for: ({chosenAssetId})");
                return;
            }

            Vector3 spawnPos = FindValidPositionNear(position);
            var spawned = Game.Instance.EntityCreator.SpawnUnit(
                blueprint,
                position,
                Quaternion.identity,
                Game.Instance.State.LoadedAreaState.MainState
            );

            if (spawned?.View == null)
            {
                Debug.LogWarning($"[Reinforcements] Spawn failed for: {chosenAssetId}");
                return;
            }

            spawned.Position = spawnPos;
            spawned.View.transform.position = spawnPos;
            spawned.View.transform.rotation = Quaternion.identity;
            spawned.CombatState.JoinCombat(true);
        }

        /// <summary>
        /// Finds a random valid position near the given origin to place a unit.
        /// </summary>
        private static Vector3 FindValidPositionNear(Vector3 origin)
        {
            float offsetX = (float)(_rng.NextDouble() * 4.0 - 2.0);
            float offsetZ = (float)(_rng.NextDouble() * 4.0 - 2.0);
            return origin + new Vector3(offsetX, 0f, offsetZ);
        }

        /// <summary>
        /// Retrieves a random unit Asset ID from the appropriate faction list that matches the specified CR.
        /// </summary>
        private static string GetRandomAssetIdByCRAndFaction(int cr, string factionId)
        {
            List<MonsterData> list = null;

            // Select the list of units corresponding to the faction
            switch (factionId)
            {
                case "28460a5d00a62b742b80c90c37559644":
                    list = BanditList.Monsters;
                    break;
                case "0f539babafb47fe4586b719d02aff7c4":
                    list = MobList.Monsters;
                    break;
                case "24a215bb66e34153b4d648829c088ae6":
                    list = OozeList.Monsters;
                    break;
                case "b1525b4b33efe0241b4cbf28486cd2cc":
                    list = WildAnimalsList.Monsters;
                    break;
                default:
                    Debug.LogWarning($"[Reinforcements] Unknown faction: {factionId}");
                    return null;
            }

            // Attempt to find a monster with matching CR, decreasing if necessary
            while (cr >= 0)
            {
                var filtered = list?.FindAll(m => m.CR == cr.ToString());
                if (filtered != null && filtered.Count > 0)
                {
                    var chosen = filtered[_rng.Next(filtered.Count)];
                    Debug.Log($"[Reinforcements] Chosen CR: {cr}, Faction: {factionId}, AssetId: {chosen.AssetId}");
                    return chosen.AssetId;
                }

                cr--; // fallback to lower CR
            }

            Debug.LogWarning($"[Reinforcements] No valid monster found for faction {factionId}");
            return null;
        }
    }
}
