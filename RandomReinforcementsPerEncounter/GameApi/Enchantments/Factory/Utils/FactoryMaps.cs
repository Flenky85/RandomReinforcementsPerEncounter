using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments.Factory.Util
{
    internal static class FactoryMaps
    {
        public static DiceType MapDiceType(int sides) => sides switch
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
            _ => DiceType.D3,
        };

        public static DamageEnergyType MapEnergyType(string s)
        {
            if (string.IsNullOrEmpty(s)) return DamageEnergyType.Fire;

            switch (s.ToLowerInvariant())
            {
                case "acid": return DamageEnergyType.Acid;
                case "fire": return DamageEnergyType.Fire;
                case "cold": return DamageEnergyType.Cold;
                case "electricity": return DamageEnergyType.Electricity;
                case "sonic": return DamageEnergyType.Sonic;
                case "negative damage": return DamageEnergyType.NegativeEnergy;
                case "holy": return DamageEnergyType.Holy;
                default: return DamageEnergyType.Fire;
            }
        }
    }
}
