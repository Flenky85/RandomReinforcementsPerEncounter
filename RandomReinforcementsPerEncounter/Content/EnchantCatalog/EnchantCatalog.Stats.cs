using Kingmaker.EntitySystem.Stats;
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
        // Nota: en tu StatRoots hay una errata: "of Strnght" → "of Strength".
        internal static readonly EnchantDef StatSTR = MakeStatBonus("statSTR", "Mighty", "of Strength", "strength", StatType.Strength);
        internal static readonly EnchantDef StatDEX = MakeStatBonus("statDEX", "Graceful", "of Agility", "dexterity", StatType.Dexterity);
        internal static readonly EnchantDef StatCON = MakeStatBonus("statCON", "Resilient", "of Constitution", "constitution", StatType.Constitution);
        internal static readonly EnchantDef StatINT = MakeStatBonus("statINT", "Cunning", "of Intelligence", "intelligence", StatType.Intelligence);
        internal static readonly EnchantDef StatWIS = MakeStatBonus("statWIS", "Sage", "of Wisdom", "wisdom", StatType.Wisdom);
        internal static readonly EnchantDef StatCHA = MakeStatBonus("statCHA", "Glamorous", "of Charisma", "charisma", StatType.Charisma);

    }
}
