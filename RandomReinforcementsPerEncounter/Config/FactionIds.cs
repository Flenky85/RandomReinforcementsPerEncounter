namespace RandomReinforcementsPerEncounter.Config
{
    /// <summary>
    /// Identificadores de facciones conocidos, usados para elegir la lista de monstruos adecuada.
    /// </summary>
    internal static class FactionIds
    {
        internal const string Bandits = "28460a5d00a62b742b80c90c37559644";
        internal const string Mob = "0f539babafb47fe4586b719d02aff7c4";
        internal const string Ooze = "24a215bb66e34153b4d648829c088ae6";
        internal const string WildAnimals = "b1525b4b33efe0241b4cbf28486cd2cc";

        /// <summary>Fallback si no se reconoce la facción.</summary>
        internal const string Unknown = "UNKNOWN";
    }
}