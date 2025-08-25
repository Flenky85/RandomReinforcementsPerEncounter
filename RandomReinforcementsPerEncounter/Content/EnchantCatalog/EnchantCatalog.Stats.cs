using Kingmaker.EntitySystem.Stats;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
        
        private static EnchantDef MakeStatBonus(
            string seed,
            string Name,  
            string suffixName,   
            string desc,         
            StatType stat)
        {
            int[] statBonusOne = { 1, 2, 2, 3, 3, 4 };
            int[] statBonusTwo = { 1, 2, 3, 4, 5, 6 };
            int chance = 10;

            return new EnchantDef
            {
                Type = EnchantType.StatsBonus,
                Seed = seed,

                Name = Name,   
                AffixDisplay = suffixName, 
                Desc = desc,               
                Affix = AffixKind.Suffix,  

                Chance = chance,

                TierMapOneHanded = statBonusOne,
                TierMapTwoHanded = statBonusTwo,

                Stat = stat      
            };
        }

        internal static readonly EnchantDef StatSTR = MakeStatBonus(Seed.statSTR, "Mighty", "of Might", "strength", StatType.Strength);
        internal static readonly EnchantDef StatDEX = MakeStatBonus(Seed.statDEX, "Graceful", "of Grace", "dexterity", StatType.Dexterity);
        internal static readonly EnchantDef StatCON = MakeStatBonus(Seed.statCON, "Resilient", "of Resilience", "constitution", StatType.Constitution);
        internal static readonly EnchantDef StatINT = MakeStatBonus(Seed.statINT, "Cunning", "of Guile", "intelligence", StatType.Intelligence);
        internal static readonly EnchantDef StatWIS = MakeStatBonus(Seed.statWIS, "Sage", "of Sagacity", "wisdom", StatType.Wisdom);
        internal static readonly EnchantDef StatCHA = MakeStatBonus(Seed.statCHA, "Glamorous", "of Glamour", "charisma", StatType.Charisma);
    }
}
