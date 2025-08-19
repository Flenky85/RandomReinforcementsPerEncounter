namespace RandomReinforcementsPerEncounter.State
{
    /// <summary>
    /// Estado efímero para proteger lógica de combate (por ejemplo, no spawnear refuerzos dos veces).
    /// </summary>
    internal static class CombatFlags
    {
        /// <summary>
        /// Marca si ya se generaron refuerzos en este combate.
        /// Se debe resetear a false al terminar el encuentro.
        /// </summary>
        internal static bool ReinforcementsSpawned { get; set; } = false;

        /// <summary>Reinicia todos los flags de combate.</summary>
        internal static void Reset()
        {
            ReinforcementsSpawned = false;
        }
    }
}