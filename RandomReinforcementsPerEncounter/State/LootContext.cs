using System.Collections.Generic;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.State
{
    /// <summary>
    /// Ephemeral state tied to the current combat (chest position and CRs of spawned enemies).
    /// </summary>
    internal static class LootContext
    {
        /// <summary>Position where to drop the chest when combat ends (if applicable).</summary>
        internal static Vector3? ChestPosition { get; set; }

        /// <summary>CR of each enemy we spawned as reinforcements.</summary>
        internal static readonly List<int> EnemyCRs = new List<int>(32);

        /// <summary>Resets the context for the next combat.</summary>
        internal static void Reset()
        {
            ChestPosition = null;
            EnemyCRs.Clear();
        }
    }
}
