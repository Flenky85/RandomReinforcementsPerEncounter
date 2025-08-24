using Kingmaker.EntitySystem.Stats;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
        private static EnchantDef MakeSkillBonus(
            string seed,
            string name,        // p.ej. "Mobile"
            string suffixName,  // p.ej. "of Mobile"
            string desc,        // p.ej. "mobility"
            StatType stat)
        {
            int[] skillBonusOne = { 2, 3, 4, 5, 6, 8 };
            int[] skillBonusTwo = { 2, 4, 6, 8, 10, 12 };
            int chance = 10;

            return new EnchantDef
            {
                Type = EnchantType.SkillsBonus,
                Seed = seed,

                // naming / UI
                Name = name,
                AffixDisplay = suffixName,
                Desc = desc,
                Affix = AffixKind.Suffix,

                Chance = chance,
                
                // mapas de valor por tier
                TierMapOneHanded = skillBonusOne,
                TierMapTwoHanded = skillBonusTwo,

                // dato específico (stat/skill)
                Stat = stat
            };
        }

        // ---- Definiciones (Skills) ----
        internal static readonly EnchantDef SkillMOB = MakeSkillBonus(Seed.skillMOB, "Mobile", "of Mobile", "mobility", StatType.SkillMobility);
        internal static readonly EnchantDef SkillATH = MakeSkillBonus(Seed.skillATH, "Vigorous", "of Vigor", "athletics", StatType.SkillAthletics);
        internal static readonly EnchantDef SkillARC = MakeSkillBonus(Seed.skillARC, "Arcane", "of the Arcanist", "knowledge arcana", StatType.SkillKnowledgeArcana);
        internal static readonly EnchantDef SkillWOR = MakeSkillBonus(Seed.skillWOR, "Scholar", "of the Scholar", "knowledge world", StatType.SkillKnowledgeWorld);
        internal static readonly EnchantDef SkillNAT = MakeSkillBonus(Seed.skillNAT, "Pathfinder", "of the Pathfinder", "lore nature", StatType.SkillLoreNature);
        internal static readonly EnchantDef SkillREL = MakeSkillBonus(Seed.skillREL, "Saintly", "of the Zealot", "lore religion", StatType.SkillLoreReligion);
        internal static readonly EnchantDef SkillPERC = MakeSkillBonus(Seed.skillPERC, "Vigilant", "of Vigilance", "perception", StatType.SkillPerception);
        internal static readonly EnchantDef SkillPERS = MakeSkillBonus(Seed.skillPERS, "Diplomatic", "of Diplomacy", "persuasion", StatType.SkillPersuasion);
        internal static readonly EnchantDef SkillSTE = MakeSkillBonus(Seed.skillSTE, "Silent", "of Silence", "stealth", StatType.SkillStealth);
        internal static readonly EnchantDef SkillTHI = MakeSkillBonus(Seed.skillTHI, "Gambit", "of Guile", "trickery", StatType.SkillThievery);
        internal static readonly EnchantDef SkillUMD = MakeSkillBonus(Seed.skillUMD, "Mystic", "of Attunement", "use magic device", StatType.SkillUseMagicDevice);
    }
}
