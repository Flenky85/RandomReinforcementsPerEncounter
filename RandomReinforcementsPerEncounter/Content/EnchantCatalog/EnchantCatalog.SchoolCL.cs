using RandomReinforcementsPerEncounter.Config.Ids.Tables;
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
            int chance = 5;

            return new EnchantDef
            {
                Type = EnchantType.SchoolCL,
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

        internal static readonly EnchantDef DivinationCL = MakeSchoolCLFeature(Seed.divinationCL, "Revealing", "caster level on divination school spells");
        internal static readonly EnchantDef EnchantmentCL = MakeSchoolCLFeature(Seed.enchantmentCL, "Amplifier", "caster level on enchantment school spells");
        internal static readonly EnchantDef EvocationCL = MakeSchoolCLFeature(Seed.evocationCL, "Blasting", "caster level on evocation school spells");
        internal static readonly EnchantDef ConjurationCL = MakeSchoolCLFeature(Seed.conjurationCL, "Summoning", "caster level on conjuration school spells");
        internal static readonly EnchantDef AbjurationCL = MakeSchoolCLFeature(Seed.abjurationCL, "Warding", "caster level on abjuration school spells");
        internal static readonly EnchantDef IllusionCL = MakeSchoolCLFeature(Seed.illusionCL, "Mirage", "caster level on illusion school spells");
        internal static readonly EnchantDef TransmutationCL = MakeSchoolCLFeature(Seed.transmutationCL, "Morphing", "caster level on transmutation school spells");
        internal static readonly EnchantDef NecromancyCL = MakeSchoolCLFeature(Seed.necromancyCL, "Grim", "caster level on necromancy school spells");
    }
}
