using System.Collections.Generic;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.State
{
    /// <summary>
    /// Estado efímero asociado al combate actual (posición del cofre y CRs de enemigos generados).
    /// </summary>
    internal static class LootContext
    {
        /// <summary>Posición donde dejar el cofre al terminar el combate (si procede).</summary>
        internal static Vector3? ChestPosition { get; set; }

        /// <summary>CR de cada enemigo que hemos spawneado como refuerzo.</summary>
        internal static readonly List<int> EnemyCRs = new List<int>(32);

        /// <summary>Reinicia el contexto para el siguiente combate.</summary>
        internal static void Reset()
        {
            ChestPosition = null;
            EnemyCRs.Clear();
        }
    }
}