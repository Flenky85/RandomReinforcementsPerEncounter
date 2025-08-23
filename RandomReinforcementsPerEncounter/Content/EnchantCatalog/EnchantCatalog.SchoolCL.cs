using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
        private static EnchantDef MakeSchoolCLFeature(
            string seed,
            string name,
            string desc)
        {
            int[] CasterBonusOne = { 1, 1, 1, 1, 2, 2 };
            int[] CasterBonusTwo = { 1, 1, 2, 2, 3, 3 };
            int chance = 10;

            return new EnchantDef
            {
                Type = EnchantType.SchoolCL,
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

        internal static readonly EnchantDef DivinationCL = MakeSchoolCLFeature("divinationCL", "Revealing", "caster level on divination school spells");
        internal static readonly EnchantDef EnchantmentCL = MakeSchoolCLFeature("enchantmentCL", "Amplifier", "caster level on enchantment school spells");
        internal static readonly EnchantDef EvocationCL = MakeSchoolCLFeature("evocationCL", "Blasting", "caster level on evocation school spells");
        internal static readonly EnchantDef ConjurationCL = MakeSchoolCLFeature("conjurationCL", "Summoning", "caster level on conjuration school spells");
        internal static readonly EnchantDef AbjurationCL = MakeSchoolCLFeature("abjurationCL", "Warding", "caster level on abjuration school spells");
        internal static readonly EnchantDef IllusionCL = MakeSchoolCLFeature("illusionCL", "Mirage", "caster level on illusion school spells");
        internal static readonly EnchantDef TransmutationCL = MakeSchoolCLFeature("transmutationCL", "Morphing", "caster level on transmutation school spells");
        internal static readonly EnchantDef NecromancyCL = MakeSchoolCLFeature("necromancyCL", "Grim", "caster level on necromancy school spells");
    }
}
