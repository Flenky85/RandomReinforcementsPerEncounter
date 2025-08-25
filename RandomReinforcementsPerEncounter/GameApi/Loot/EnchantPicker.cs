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

        public static (BlueprintItemEnchantment enchant, bool duplicateToSecond) PickWeighted(
                    int tier,
                    WeaponGrip weaponGrip,
                    bool isRanged,
                    AffixKind affix,
                    ISet<string> usedIds)
        {

            var raw = LootBuckets.GetCandidatesByTierAndAffix(tier, affix).ToList();

            IEnumerable<(string id, int weight, WeaponGrip hand, bool applyBothOnDouble)> pool;

            switch (weaponGrip)
            {
                case WeaponGrip.Double:
                    {
                        var bothHeads = raw.Where(c => c.hand == WeaponGrip.OneHanded && c.applyBothOnDouble).ToList();
                        if (bothHeads.Count > 0)
                        {
                            pool = bothHeads;
                        }
                        else
                        {
                            pool = raw.Where(c => c.hand == WeaponGrip.TwoHanded);
                        }
                        break;
                    }

                case WeaponGrip.TwoHanded:
                    pool = raw.Where(c => c.hand == WeaponGrip.TwoHanded);
                    break;

                default: 
                    pool = raw.Where(c => c.hand == WeaponGrip.OneHanded);
                    break;
            }

            if (usedIds != null) pool = pool.Where(x => !usedIds.Contains(x.id));

            var list = pool.ToList();
            if (list.Count == 0) return (null, false);

            int total = 0;
            foreach (var c in list) total += Math.Max(1, c.weight);
            int roll = UnityEngine.Random.Range(1, total + 1);

            int acc = 0;
            (string id, int weight, WeaponGrip hand, bool applyBothOnDouble) pick = default;
            foreach (var c in list)
            {
                acc += Math.Max(1, c.weight);
                if (roll <= acc) { pick = c; break; }
            }

            if (string.IsNullOrEmpty(pick.id)) return (null, false);

            var bp = LootUtils.TryLoadEnchant(pick.id);
            if (bp == null) return (null, false);

            bool duplicateToSecond = (weaponGrip == WeaponGrip.Double) &&
                                     (pick.hand == WeaponGrip.OneHanded) &&
                                     pick.applyBothOnDouble;

            return (bp, duplicateToSecond);
        }


    }
}
