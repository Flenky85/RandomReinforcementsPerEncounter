using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Items;
using RandomReinforcementsPerEncounter.GameApi.Mechanics;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    internal static class EnchantApplier
    {
        private static bool HasEnchant(ItemEntity item, BlueprintItemEnchantment ench)
            => item?.Enchantments?.Any(e => e?.Blueprint == ench) == true;

        public static void AddEnchants(ItemEntityWeapon entity, bool applyToSecond, params BlueprintItemEnchantment[] enchants)
        {
            if (entity == null || enchants == null || enchants.Length == 0) return;

            var ctx = RRECtx.Permanent();

            foreach (var e in enchants)
                if (e != null)
                    entity.AddEnchantment(e, ctx);

            if (applyToSecond && entity.Second != null)
            {
                var second = entity.Second;
                foreach (var e in enchants)
                {
                    if (e == null) continue;
                    if (HasEnchant(second, e)) continue;
                    second.AddEnchantment(e, ctx);
                }
            }
        }

        private static BlueprintItemEnchantment PriceForTier(int tier) => tier switch
        {
            1 => PriceRefs.PriceT1,
            2 => PriceRefs.PriceT2,
            3 => PriceRefs.PriceT3,
            4 => PriceRefs.PriceT4,
            5 => PriceRefs.PriceT5,
            6 => PriceRefs.PriceT6,
            _ => PriceRefs.PriceT1,
        };

        public static void ApplyRandomTierEnchant(ItemEntityWeapon entity, int tier, int[] chances)
        {
            var baseEnchant = LootUtils.TryLoadEnchant(LootRefs.GetWeaponEnchantIdForTier(tier));
            AddEnchants(entity, true, baseEnchant, PriceForTier(tier));

            WeaponGrip weaponGrip =
                (entity?.Blueprint?.SecondWeapon != null) ? WeaponGrip.Double :
                (entity?.Blueprint?.IsTwoHanded == true) ? WeaponGrip.TwoHanded :
                WeaponGrip.OneHanded;

            bool isRanged = entity?.Blueprint?.IsRanged == true;

            var usedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            int[] limitedChances = (chances == null || tier <= 0) ? Array.Empty<int>() : chances.Take(tier).ToArray();
            if (limitedChances.Length == 0) return;

            AffixKind desiredAffix = UnityEngine.Random.value < 0.5f ? AffixKind.Prefix : AffixKind.Suffix;

            for (int i = 0; i < tier; i++)
            {
                int enchantTier = TierChances.GetRandomTier(limitedChances);

                var pick = EnchantPicker.PickWeighted(
                    enchantTier,
                    weaponGrip,
                    isRanged,
                    desiredAffix,
                    usedIds
                );

                if (pick.enchant == null)
                {
                    UnityEngine.Debug.Log($"[RRE] Empty pool at iter {i + 1}/{tier} (T{enchantTier}, {weaponGrip}, {(isRanged ? "Ranged" : "Melee")}, {desiredAffix}).");
                    break;
                }

                string appliedId = pick.enchant.AssetGuid.ToString();
                if (HasEnchant(entity, pick.enchant) || usedIds.Contains(appliedId))
                {
                    i--;
                    usedIds.Add(appliedId);
                    continue;
                }

                AddEnchants(entity, pick.duplicateToSecond, pick.enchant);
                usedIds.Add(appliedId);

                desiredAffix = (desiredAffix == AffixKind.Prefix) ? AffixKind.Suffix : AffixKind.Prefix;
            }
        }
    }
}
