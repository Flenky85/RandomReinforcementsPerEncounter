using System.Collections.Generic;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.State
{

    internal static class ReinforcementState
    {
        internal static readonly List<(Vector3 Position, int CR, string FactionId)> Pending
            = new List<(Vector3 Position, int CR, string FactionId)>();

        internal static bool HasPending => Pending.Count > 0;


        internal static void AddPending(Vector3 position, int cr, string factionId)
            => Pending.Add((position, cr, factionId));

        internal static void Clear() => Pending.Clear();

        internal static void TrySpawnPendingReinforcements()
        {
            if (!HasPending) return;

            foreach (var r in Pending)
                GameApi.ReinforcementSpawner.SpawnReinforcementAt(r.Position, r.CR, r.FactionId);

            Pending.Clear();
        }
    }
}
