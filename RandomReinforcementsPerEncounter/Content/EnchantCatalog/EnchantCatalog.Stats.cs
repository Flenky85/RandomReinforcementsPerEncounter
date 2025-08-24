using Kingmaker.EntitySystem.Stats;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
        // Dentro de EnchantCatalog (el mismo sitio donde tienes StatRoots y los arrays)
        private static EnchantDef MakeStatBonus(
            string seed,
            string Name,   // p.ej. "Mighty"
            string suffixName,   // p.ej. "of Strength"
            string desc,         // p.ej. "strength"
            StatType stat)
        {
            // Usa tus tablas por tier
            int[] statBonusOne = { 1, 2, 2, 3, 3, 4 };
            int[] statBonusTwo = { 1, 2, 3, 4, 5, 6 };
            int chance = 10;

            return new EnchantDef
            {
                Type = EnchantType.StatsBonus,
                Seed = seed,

                // naming / UI
                Name = Name,          // nombre "corto" interno (prefijo)
                AffixDisplay = suffixName,  // lo que se muestra como sufijo: "of Strength"
                Desc = desc,                // texto base: "strength"
                Affix = AffixKind.Suffix,   // estos van como sufijo

                Chance = chance,

                // el builder reutiliza estos mapas como "valor" por tier
                TierMapOneHanded = statBonusOne,
                TierMapTwoHanded = statBonusTwo,

                // dato específico de stats
                Stat = stat                  // <-- añade este campo en EnchantDef si aún no existe (StatType Stat)
            };
        }

        // ---- Definiciones (StatsBonus) ----
        internal static readonly EnchantDef StatSTR = MakeStatBonus(Seed.statSTR, "Mighty", "of Might", "strength", StatType.Strength);
        internal static readonly EnchantDef StatDEX = MakeStatBonus(Seed.statDEX, "Graceful", "of Grace", "dexterity", StatType.Dexterity);
        internal static readonly EnchantDef StatCON = MakeStatBonus(Seed.statCON, "Resilient", "of Resilience", "constitution", StatType.Constitution);
        internal static readonly EnchantDef StatINT = MakeStatBonus(Seed.statINT, "Cunning", "of Guile", "intelligence", StatType.Intelligence);
        internal static readonly EnchantDef StatWIS = MakeStatBonus(Seed.statWIS, "Sage", "of Sagacity", "wisdom", StatType.Wisdom);
        internal static readonly EnchantDef StatCHA = MakeStatBonus(Seed.statCHA, "Glamorous", "of Glamour", "charisma", StatType.Charisma);
    }
}
