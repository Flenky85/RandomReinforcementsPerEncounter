using RandomReinforcementsPerEncounter.Config.Ids.Generators;
using RandomReinforcementsPerEncounter.Domain.Models;


namespace RandomReinforcementsPerEncounter
{
    internal static partial class LootBuckets
    {
        public static void AddRootVariant(
            EnchantType type,
            string root,
            WeaponGrip hand,
            AffixKind affix,
            int? value = null,
            bool applyToBothHeadsOnDouble = false)
        {
            int w = value ?? _defaultValue;
            var tiers = GetBucket(type, w);

            for (int t = 1; t <= MaxTier; t++)
            {
                var guid = EnchantIds.Id(root, t, hand);
                tiers[t - 1].Add(guid);

                _handById[guid] = hand;
                _affixById[guid] = affix;

                if (hand == WeaponGrip.OneHanded)
                    _applyBothOnDoubleById[guid] = applyToBothHeadsOnDouble;
            }
        }

    }
}
