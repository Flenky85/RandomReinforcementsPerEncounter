using Kingmaker.Blueprints;
using Kingmaker.Items;
using Kingmaker.View.MapObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Kingmaker.Blueprints.Items.Ecnchantments;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;


namespace RandomReinforcementsPerEncounter.GameApi.Chest
{
    internal static class EnhancementLookup
    {
        public static readonly Dictionary<BlueprintGuid, int> PlusByGuid =
            new Dictionary<BlueprintGuid, int>
            {
                // +1
                [BlueprintGuid.Parse(BlueprintGuids.WeaponPlus1)] = 1,
                [BlueprintGuid.Parse(BlueprintGuids.ArmorPlus1)] = 1,
                [BlueprintGuid.Parse(BlueprintGuids.ShieldPlus1)] = 1,
                // +2
                [BlueprintGuid.Parse(BlueprintGuids.WeaponPlus2)] = 2,
                [BlueprintGuid.Parse(BlueprintGuids.ArmorPlus2)] = 2,
                [BlueprintGuid.Parse(BlueprintGuids.ShieldPlus2)] = 2,
                // +3
                [BlueprintGuid.Parse(BlueprintGuids.WeaponPlus3)] = 3,
                [BlueprintGuid.Parse(BlueprintGuids.ArmorPlus3)] = 3,
                [BlueprintGuid.Parse(BlueprintGuids.ShieldPlus3)] = 3,
                // +4
                [BlueprintGuid.Parse(BlueprintGuids.WeaponPlus4)] = 4,
                [BlueprintGuid.Parse(BlueprintGuids.ArmorPlus4)] = 4,
                [BlueprintGuid.Parse(BlueprintGuids.ShieldPlus4)] = 4,
                // +5
                [BlueprintGuid.Parse(BlueprintGuids.WeaponPlus5)] = 5,
                [BlueprintGuid.Parse(BlueprintGuids.ArmorPlus5)] = 5,
                [BlueprintGuid.Parse(BlueprintGuids.ShieldPlus5)] = 5,
                // +6
                [BlueprintGuid.Parse(BlueprintGuids.WeaponPlus6)] = 6,
                [BlueprintGuid.Parse(BlueprintGuids.ArmorPlus6)] = 6,
                [BlueprintGuid.Parse(BlueprintGuids.ShieldPlus6)] = 6,
            };

        public static int GetWeaponEnhancementPlus(ItemEntityWeapon w)
        {
            if (w?.Enchantments == null) return 0;

            int maxPlus = 0;
            foreach (var e in w.Enchantments)
            {
                var bp = e?.Blueprint as BlueprintWeaponEnchantment;
                if (bp == null) continue;

                if (PlusByGuid.TryGetValue(bp.AssetGuid, out int plus) && plus > maxPlus)
                    maxPlus = plus;
            }
            return Mathf.Clamp(maxPlus, 0, 6);
        }

    }

    internal static class ChestVisuals
    {
        public static void ApplyScaleFromLoot(MapObjectView view, InteractionLootPart lootPart)
        {
            if (view == null || lootPart?.Loot?.Items == null) return;

            int maxPlus = lootPart.Loot.Items
                .OfType<ItemEntityWeapon>()
                .Select(EnhancementLookup.GetWeaponEnhancementPlus)
                .DefaultIfEmpty(0)
                .Max();

            float s = ChestScale.ForPlus(maxPlus);
            view.transform.localScale = new Vector3(s, s, s);
        }
    }
}