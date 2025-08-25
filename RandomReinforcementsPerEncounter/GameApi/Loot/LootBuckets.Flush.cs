using RandomReinforcementsPerEncounter.Domain.Models;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class LootBuckets
    {
        public static void FlushToEnchantList(List<EnchantData> target)
        {
            target.Clear();

            foreach (var byType in _store)
            {
                var type = byType.Key;

                foreach (var byValue in byType.Value)
                {
                    int value = byValue.Key;
                    var tiers = byValue.Value; 
                    if (IsAllEmpty(tiers)) continue;

                    var perHand = new Dictionary<WeaponGrip, List<string>[]> {
                        { WeaponGrip.OneHanded, NewTierLists() },
                        { WeaponGrip.TwoHanded, NewTierLists()  },
                        { WeaponGrip.Double,    NewTierLists()  },
                    };

                    for (int i = 0; i < MaxTier; i++)
                    {
                        foreach (var id in tiers[i])
                        {
                            var hand = _handById.TryGetValue(id, out var h) ? h : WeaponGrip.OneHanded;
                            perHand[hand][i].Add(id);
                        }
                    }

                    foreach (var kv in perHand)
                    {
                        var hand = kv.Key;
                        var arrs = kv.Value;

                        if (IsAllEmpty(arrs)) continue;

                        target.Add(new EnchantData
                        {
                            AssetIDT1 = arrs[0].ToArray(),
                            AssetIDT2 = arrs[1].ToArray(),
                            AssetIDT3 = arrs[2].ToArray(),
                            AssetIDT4 = arrs[3].ToArray(),
                            AssetIDT5 = arrs[4].ToArray(),
                            AssetIDT6 = arrs[5].ToArray(),
                            Value = value,
                            Type = type,
                            Hand = hand
                        });
                    }
                }
            }
        }
    }
}
