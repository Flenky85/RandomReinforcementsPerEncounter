using System.Collections.Generic;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.State
{
    /// <summary>
    /// Cola de refuerzos pendientes de spawnear durante el combate.
    /// </summary>
    internal static class ReinforcementState
    {
        internal static readonly List<(Vector3 Position, int CR, string FactionId)> Pending
            = new List<(Vector3 Position, int CR, string FactionId)>();

        internal static bool HasPending => Pending.Count > 0;

        /// <summary>Añade un refuerzo a la cola.</summary>
        internal static void AddPending(Vector3 position, int cr, string factionId)
            => Pending.Add((position, cr, factionId));

        /// <summary>Limpia la cola (por ejemplo, al terminar el combate).</summary>
        internal static void Clear() => Pending.Clear();

        /// <summary>Spawnea todos los refuerzos pendientes y vacía la cola.</summary>
        internal static void TrySpawnPendingReinforcements()
        {
            if (!HasPending) return;

            // Si usas reservas de spawn para dispersión, puedes limpiar aquí:
            // RandomReinforcementsPerEncounter.SpawnReservations.Clear();

            foreach (var r in Pending)
                GameApi.ReinforcementSpawner.SpawnReinforcementAt(r.Position, r.CR, r.FactionId);

            Pending.Clear();
        }
    }
}
