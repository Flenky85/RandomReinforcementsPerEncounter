using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
        private static EnchantDef MakeSchoolDCFeature(
            string seed,
            string name,
            string desc)
        {
            int[] CasterBonusOne = { 1, 1, 1, 1, 2, 2 };
            int[] CasterBonusTwo = { 1, 1, 2, 2, 3, 3 };
            int chance = 5;

            return new EnchantDef
            {
                Type = EnchantType.SchoolDC,
                Seed = seed,

                Name = name,
                AffixDisplay = name,          
                Desc = desc,
                Affix = AffixKind.Prefix,     

                Chance = chance,

                TierMapOneHanded = CasterBonusOne,
                TierMapTwoHanded = CasterBonusTwo,

            };
        }


        internal static readonly EnchantDef DivinationDC = MakeSchoolDCFeature(Seed.divinationDC, "Insightful", "DC on divination school spells");
        internal static readonly EnchantDef EnchantmentDC = MakeSchoolDCFeature(Seed.enchantmentDC, "Mesmeric", "DC on enchantment school spells");
        internal static readonly EnchantDef EvocationDC = MakeSchoolDCFeature(Seed.evocationDC, "Cataclysmic", "DC on evocation school spells");
        internal static readonly EnchantDef ConjurationDC = MakeSchoolDCFeature(Seed.conjurationDC, "Binding", "DC on conjuration school spells");
        internal static readonly EnchantDef AbjurationDC = MakeSchoolDCFeature(Seed.abjurationDC, "Repelling", "DC on abjuration school spells");
        internal static readonly EnchantDef IllusionDC = MakeSchoolDCFeature(Seed.illusionDC, "Chimeric", "DC on illusion school spells");
        internal static readonly EnchantDef TransmutationDC = MakeSchoolDCFeature(Seed.transmutationDC, "Mutable", "DC on transmutation school spells");
        internal static readonly EnchantDef NecromancyDC = MakeSchoolDCFeature(Seed.necromancyDC, "Deathly", "DC on necromancy school spells");
    }
}
