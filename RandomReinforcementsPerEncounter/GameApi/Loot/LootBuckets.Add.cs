using RandomReinforcementsPerEncounter.Domain.Models;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class LootBuckets
    {
        public static void AddRootVariant(EnchantType type, string root, WeaponGrip hand, AffixKind affix, int? value = null)
        {
            int val = value ?? _defaultValue;
            var tiers = GetBucket(type, val);

            for (int t = 1; t <= MaxTier; t++)
            {
                var seed = RootWithHand(root, hand) + ".t" + t;
                var guid = GuidUtil.EnchantGuid(seed).ToString();
                tiers[t - 1].Add(guid);

                _handById[guid] = hand;   // etiqueta variante
                _affixById[guid] = affix; // etiqueta affix
            }
        }
    }
}
