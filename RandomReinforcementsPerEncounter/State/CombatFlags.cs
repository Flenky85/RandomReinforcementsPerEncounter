namespace RandomReinforcementsPerEncounter.State
{
    /// <summary>
    /// Ephemeral state used to guard combat logic (e.g., avoid spawning reinforcements twice).
    /// </summary>
    internal static class CombatFlags
    {
        /// <summary>
        /// Marks whether reinforcements have already been generated in this combat.
        /// Must be reset to false when the encounter ends.
        /// </summary>
        internal static bool ReinforcementsSpawned { get; set; } = false;

        /// <summary>Resets all combat flags.</summary>
        internal static void Reset()
        {
            ReinforcementsSpawned = false;
        }
    }
}
