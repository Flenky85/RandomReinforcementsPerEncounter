using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments.Factory.Utils
{
    internal static class FactoryMaps
    {
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
