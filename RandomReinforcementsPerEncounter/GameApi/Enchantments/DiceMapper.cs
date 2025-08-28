using Kingmaker.RuleSystem; 

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments
{
    internal static class DiceMapper
    {
        public static DiceType MapDiceType(int sides) => (DiceType)sides;

    }
}
