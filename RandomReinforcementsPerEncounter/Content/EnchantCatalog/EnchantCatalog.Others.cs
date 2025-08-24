using Kingmaker.EntitySystem.Stats;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
        private static EnchantDef MakeOtherBonus(
            string seed,
            string name,
            string suffixName,
            string desc,
            StatType stat)
        {
            int[] BonusOne = { 1, 2, 2, 3, 3, 4 };
            int[] BonusTwo = { 1, 2, 3, 4, 5, 6 };
            int chance = 10;

            return new EnchantDef
            {
                Type = EnchantType.Others,   
                Seed = seed,

                Name = name,
                AffixDisplay = suffixName,
                Desc = desc,
                Affix = AffixKind.Suffix,

                Chance = chance,

                TierMapOneHanded = BonusOne,
                TierMapTwoHanded = BonusTwo,

                Stat = stat
            };
        }

        // --- Definiciones (Others) ---
        // Maneuvers
        internal static readonly EnchantDef CMB = MakeOtherBonus(Seed.CMB, "Grappling", "of Grapple", "combat maneuver bonus", StatType.AdditionalCMB);
        internal static readonly EnchantDef CMD = MakeOtherBonus(Seed.CMD, "Immovable", "of Immovable", "combat maneuver defense", StatType.AdditionalCMD);

        // Initiative
        internal static readonly EnchantDef Initiative = MakeOtherBonus(Seed.initiative, "Swift", "of Alacrity", "initiative", StatType.Initiative);

        // Saves
        internal static readonly EnchantDef SaveFOR = MakeOtherBonus(Seed.saveFOR, "Enduring", "of Endurance", "fortitude saving throw", StatType.SaveFortitude);
        internal static readonly EnchantDef SaveWIL = MakeOtherBonus(Seed.saveWIL, "Ironwill", "of Willpower", "will saving throw", StatType.SaveWill);
        internal static readonly EnchantDef SaveREF = MakeOtherBonus(Seed.saveREF, "Elusive", "of Evasion", "reflex saving throw", StatType.SaveReflex);

    }
}
