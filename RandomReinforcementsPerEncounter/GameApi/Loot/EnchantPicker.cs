using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints.Items.Ecnchantments;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    internal static class EnchantPicker
    {
        private static string[] GetTierArray(EnchantData d, int tier) => tier switch
        {
            1 => d.AssetIDT1,
            2 => d.AssetIDT2,
            3 => d.AssetIDT3,
            4 => d.AssetIDT4,
            5 => d.AssetIDT5,
            6 => d.AssetIDT6,
            _ => null
        };

        private static EnchantType GetEnchantTypeByAssetId(string assetId)
        {
            foreach (var d in EnchantList.Item)
            {
                if (d.AssetIDT1 != null && Array.IndexOf(d.AssetIDT1, assetId) >= 0) return d.Type;
                if (d.AssetIDT2 != null && Array.IndexOf(d.AssetIDT2, assetId) >= 0) return d.Type;
                if (d.AssetIDT3 != null && Array.IndexOf(d.AssetIDT3, assetId) >= 0) return d.Type;
                if (d.AssetIDT4 != null && Array.IndexOf(d.AssetIDT4, assetId) >= 0) return d.Type;
                if (d.AssetIDT5 != null && Array.IndexOf(d.AssetIDT5, assetId) >= 0) return d.Type;
                if (d.AssetIDT6 != null && Array.IndexOf(d.AssetIDT6, assetId) >= 0) return d.Type;
            }
            return EnchantType.Others;
        }

        public static BlueprintItemEnchantment PickRandomEnchantByTier(
            int tier,
            WeaponGrip WeaponGripType,
            out EnchantType pickedType,
            HashSet<string> excludeIds)
        {
            pickedType = EnchantType.Others;
            var pool = new List<(string id, int weight)>();

            foreach (var d in EnchantList.Item)
            {
                if (d == null || d.Value <= 0) continue;
                var arr = GetTierArray(d, tier);
                if (arr == null || arr.Length == 0) continue;

                foreach (var id in arr)
                    if (!string.IsNullOrEmpty(id) && (excludeIds == null || !excludeIds.Contains(id)))
                        pool.Add((id, d.Value));
            }

            int total = pool.Sum(p => p.weight);
            if (total <= 0) return null;

            int roll = UnityEngine.Random.Range(0, total);
            int acc = 0;
            string pickedId = null;

            foreach (var p in pool)
            {
                acc += p.weight;
                if (roll < acc) { pickedId = p.id; break; }
            }
            if (string.IsNullOrEmpty(pickedId)) return null;

            pickedType = GetEnchantTypeByAssetId(pickedId);
            return LootUtils.TryLoadEnchant(pickedId);
        }

        // wrapper sin excludeIds (para usos antiguos)
        public static BlueprintItemEnchantment PickRandomEnchantByTier(
            int tier, WeaponGrip WeaponGripType, out EnchantType pickedType)
            => PickRandomEnchantByTier(tier, WeaponGripType, out pickedType, null);
    }
}
