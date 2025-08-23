using Kingmaker.EntitySystem.Stats; // SavingThrowType

namespace RandomReinforcementsPerEncounter.Domain.Models
{
    internal sealed class EnchantDef
    {
        public string Seed;              // "spellDC"
        public string Name;              // "Hexing"
        public string AffixDisplay;      // etiqueta visible del affix
        public string Desc;              // texto largo/condición
        public AffixKind Affix;          // Prefix / Suffix
        public EnchantType Type;         // CasterGeneral, OnHit, etc.
        public int Chance;               // loot Chance

        // Double
        public bool ApplyToBothHeadsOnDouble = false;    // True to apply one-handed enchant in two parts

        // Mapas por tier (si aplica)
        public int[] TierMapOneHanded;   // len 6
        public int[] TierMapTwoHanded;   // len 6

        // Solo OnHit / OnlyOnFirstHit
        public string OnHitBuffBlueprintId;
        public SavingThrowType? OnHitSave;
        public int? OnHitDurDiceCount;
        public int? OnHitDurDiceSides;

        // --- Solo para EnergyDamage ---
        public (int dice, int sides)[] DamageMapOneHanded; // len 6
        public (int dice, int sides)[] DamageMapTwoHanded; // len 6
        public string DamagePrefab;                        // EnchantsPrefabs.Flaming, etc.

        // Stats
        public StatType Stat;
    }
}
