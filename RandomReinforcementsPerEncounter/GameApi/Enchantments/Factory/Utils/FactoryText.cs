using Kingmaker.EntitySystem.Stats;

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments.Factory.Utils
{
    internal static class FactoryText
    {
        public static string BuildDebuffDescription(
            SavingThrowType saveType,
            int dc,
            string conditionText,
            int durationCount,
            int durationSides,
            bool onlyOnFirstHit
)
        {
            string intro = onlyOnFirstHit
                ? "The first time this weapon hits a given enemy, that enemy must pass a "
                : "Whenever this weapon lands a hit, the enemy must pass a ";

            string saveName = saveType.ToString().ToLowerInvariant();
            string diceText = durationSides == 1 ? durationCount.ToString() : $"{durationCount}d{durationSides}";
            bool isExactlyOneRound = durationSides == 1 && durationCount == 1;
            string roundText = isExactlyOneRound ? "round" : "rounds";

            string plain = $"{intro}{saveName} saving throw (DC {dc}) or become {conditionText} for {diceText} {roundText}.";
            return plain;
        }

        public static string BuildEnergyDescription(int diceCount, int diceSides, string energyWord)
        {
            string diceText = diceSides == 1 ? diceCount.ToString() : $"{diceCount}d{diceSides}";
            string plain = $"This weapon deals an extra {diceText} points of {energyWord} on a successful hit.";
            return plain;
        }

        public static string BuildStackableBonusDescription(int bonus, string description)
        {
            string plain = $"This item grants a +{bonus} bonus to {description}.";
            return plain;
        }
    }
}
