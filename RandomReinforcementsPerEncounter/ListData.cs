using System.Security.Permissions;

namespace RandomReinforcementsPerEncounter
{
    public class MonsterData
    {
        public string AssetId;
        public string Levels;
        public string CR;
        public string Faction;
    }
    public class ItemData
    {
        public string AssetId;
        public string Name;
        public string Cost;
        public string CR;
        public string IsNotable;
    }

    public enum WeaponType
    {
        OneHandedMelee,
        TwoHandedMelee,
        OneHandedRanged,
        TwoHandedRanged,
        Double
    }
    public enum WeaponFocusMod
    {
        Axe,
        Bow,
        Hammers,
        HeavyBlades,
        LightBlades,
        Polearm,
        Spears,
        Close,
        Crossbows,
        Monk,
        Thrown,
        Double,
    }
    public class WeaponLootData
    {
        public string AssetId;
        public string Name;
        public WeaponType Type;
        public WeaponFocusMod Focus;
    }
    public class WeaponOverLootData
    {
        public string AssetId;
        public string Original;
        public string Name;
        public WeaponType Type;
        public WeaponFocusMod Focus;
    }

    public enum EnchantType
    {
        OnHit,
        OnlyOnFirstHit,
        EnergyDamage,
        StatsBonus,
        SavesBonus,
        SkillsBonus,
        Others,
        Caster,
        SchoolCL,
        SchoolDC

    }
    public class EnchantData
    {
        public string[] AssetIDT1;
        public string[] AssetIDT2;
        public string[] AssetIDT3;
        public string[] AssetIDT4;
        public string[] AssetIDT5;
        public string[] AssetIDT6;
        public int Value;
        public EnchantType Type;
    }
}
