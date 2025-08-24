namespace RandomReinforcementsPerEncounter.Config
{
    // Valores de balance para selección de armas
    internal static class WeaponLootBalance
    {
        // Pesos (suman lo que quieras; se normalizan en runtime)
        public const int OneHandedMelee = 0;
        public const int TwoHandedMelee = 0;
        public const int OneHandedRanged = 0;
        public const int TwoHandedRanged = 0;
        public const int Double = 10;

        // Probabilidad por defecto de elegir “oversized”
        public const float OversizedChance = 0.15f;
    }
}