using Kingmaker.RuleSystem; // DiceType

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments
{
    internal static class DiceMapper
    {
        public static DiceType MapDiceType(int sides)
        {
            return sides switch
            {
                1 => DiceType.One,
                3 => DiceType.D3,
                4 => DiceType.D4,
                6 => DiceType.D6,
                8 => DiceType.D8,
                10 => DiceType.D10,
                12 => DiceType.D12,
                20 => DiceType.D20,
                100 => DiceType.D100,
                _ => DiceType.D3, // fallback conservador
            };
        }
    }
}
