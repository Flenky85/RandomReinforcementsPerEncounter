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
}
