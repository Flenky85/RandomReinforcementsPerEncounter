using Kingmaker.EntitySystem.Stats;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
        private static EnchantDef MakeCasterBonus(
            string seed,
            string name,           
            string desc)
        {
            int[] CasterBonusOne = { 1, 1, 1, 1, 2, 2 };
            int[] CasterBonusTwo = { 1, 1, 2, 2, 3, 3 };
            int chance = 10;

            return new EnchantDef
            {
                Type = EnchantType.Caster,   
                Seed = seed,

                Name = name,
                AffixDisplay = name,
                Desc = desc,
                Affix = AffixKind.Prefix,

                Chance = chance,

                TierMapOneHanded = CasterBonusOne,
                TierMapTwoHanded = CasterBonusTwo,

                Stat = StatType.BonusCasterLevel
            };
        }

        internal static readonly EnchantDef CasterLevel = MakeCasterBonus(Seed.casterLevel, "Eldritch", "caster level");
    }
}
