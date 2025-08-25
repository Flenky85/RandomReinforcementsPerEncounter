using Kingmaker.EntitySystem.Stats; // SavingThrowType

namespace RandomReinforcementsPerEncounter.Domain.Models
{
    internal sealed class EnchantDef
    {
        public string Seed;              // Deterministic key (e.g., "spellDC")
        public string Name;              // Base display name (e.g., "Hexing")
        public string AffixDisplay;      // Visible affix label
        public string Desc;              // Long text / conditions
        public AffixKind Affix;          // Prefix / Suffix
        public EnchantType Type;         // CasterGeneral, OnHit, etc.
        public int Chance;               // Loot chance / weight

        // Double weapons
        public bool ApplyToBothHeadsOnDouble = false;    // Apply 1H enchant to both heads

        // Tier maps (if applicable)
        public int[] TierMapOneHanded;   // len 6 (T1..T6)
        public int[] TierMapTwoHanded;   // len 6 (T1..T6)

        // Only for OnHit / OnlyOnFirstHit
        public string OnHitBuffBlueprintId;              // Buff AssetId to apply on hit
        public SavingThrowType? OnHitSave;               // Optional save type
        public int? OnHitDurDiceCount;                   // Duration N of NdS
        public int? OnHitDurDiceSides;                   // Duration S of NdS

        // --- Only for EnergyDamage ---
        public (int dice, int sides)[] DamageMapOneHanded; // len 6 (dice,sides per tier)
        public (int dice, int sides)[] DamageMapTwoHanded; // len 6 (dice,sides per tier)
        public string DamagePrefab;                        // VFX/SFX hint (e.g., EnchantsPrefabs.Flaming)

        // Stats
        public StatType Stat;            // Affected stat when Type implies a stat bonus
    }
}
