using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
        private static EnchantDef MakeCasterFeature(
            string seed, 
            string name, 
            string desc)
        {
            int[] CasterBonusOne = { 1, 1, 1, 1, 2, 2 };
            int[] CasterBonusTwo = { 1, 1, 2, 2, 3, 3 };
            int chance = 5;

            return new EnchantDef
            {
                Type = EnchantType.CasterFeature,
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

        internal static readonly EnchantDef SpellDC = MakeCasterFeature(Seed.spellDC, "Hexing", "spell DC for all saving throws against spells the wielder casts");
        internal static readonly EnchantDef SpellDieBonus = MakeCasterFeature(Seed.spellDieBonus, "Overcharged", "each die rolled when casting a spell with descriptor fire, cold, electricity, acid, sonic, force or cure");

    }
}
