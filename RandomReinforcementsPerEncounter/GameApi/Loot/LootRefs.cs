﻿using Kingmaker.Blueprints;
using RandomReinforcementsPerEncounter.Config.Ids;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    internal static class LootRefs
    {
        // Materiales / calidades
        public static readonly BlueprintGuid Druchite = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.Druchite);
        public static readonly BlueprintGuid ColdIron = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.ColdIron);
        public static readonly BlueprintGuid Mithral = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.Mithral);
        public static readonly BlueprintGuid Adamantine = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.Adamantine);

        public static readonly BlueprintGuid MasterWork = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.MasterWork);
        public static readonly BlueprintGuid Composite = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.Composite);

        // Enhancements arma +X
        public static readonly BlueprintGuid Weapon1 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus1);
        public static readonly BlueprintGuid Weapon2 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus2);
        public static readonly BlueprintGuid Weapon3 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus3);
        public static readonly BlueprintGuid Weapon4 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus4);
        public static readonly BlueprintGuid Weapon5 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus5);
        public static readonly BlueprintGuid Weapon6 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus6);

        public static BlueprintGuid GetWeaponEnchantIdForTier(int tier) => tier switch
        {
            1 => Weapon1,
            2 => Weapon2,
            3 => Weapon3,
            4 => Weapon4,
            5 => Weapon5,
            6 => Weapon6,
            _ => Weapon1
        };
    }
}
