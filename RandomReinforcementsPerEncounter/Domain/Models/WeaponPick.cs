namespace RandomReinforcementsPerEncounter.Domain.Models
{
    internal struct WeaponPick
    {
        public string Name;
        public string AssetId;
        public WeaponType Type;
        public WeaponFocusMod Focus;
        public bool IsOversized;
    }
}
