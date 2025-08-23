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
            int chance = 10;

            return new EnchantDef
            {
                Type = EnchantType.SchoolDC,
                Seed = seed,

                Name = name,
                AffixDisplay = name,          // prefijo visible
                Desc = desc,
                Affix = AffixKind.Prefix,     // estos van como prefijo

                Chance = chance,

                TierMapOneHanded = CasterBonusOne,
                TierMapTwoHanded = CasterBonusTwo,

            };
        }

        
        internal static readonly EnchantDef DivinationDC = MakeSchoolDCFeature("divinationDC", "Insightful", "DC on divination school spells");
        internal static readonly EnchantDef EnchantmentDC = MakeSchoolDCFeature("enchantmentDC", "Mesmeric", "DC on enchantment school spells");
        internal static readonly EnchantDef EvocationDC = MakeSchoolDCFeature("evocationDC", "Cataclysmic", "DC on evocation school spells");
        internal static readonly EnchantDef ConjurationDC = MakeSchoolDCFeature("conjurationDC", "Binding", "DC on conjuration school spells");
        internal static readonly EnchantDef AbjurationDC = MakeSchoolDCFeature("abjurationDC", "Repelling", "DC on abjuration school spells");
        internal static readonly EnchantDef IllusionDC = MakeSchoolDCFeature("illusionDC", "Chimeric", "DC on illusion school spells");
        internal static readonly EnchantDef TransmutationDC = MakeSchoolDCFeature("transmutationDC", "Mutable", "DC on transmutation school spells");
        internal static readonly EnchantDef NecromancyDC = MakeSchoolDCFeature("necromancyDC", "Deathly", "DC on necromancy school spells");
    }
}
