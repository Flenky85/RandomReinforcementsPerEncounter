using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Items;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomReinforcementsPerEncounter.GameApi.Items
{
    internal static class ItemNameFormatter
    {
        private static readonly string[] MaterialIds =
        {
            BlueprintGuids.MasterWork,
            BlueprintGuids.Druchite,
            BlueprintGuids.ColdIron,
            BlueprintGuids.Mithral,
            BlueprintGuids.Adamantine,
            BlueprintGuids.Composite,
        };

        private static readonly string[] WeaponEnhancementIds =
        {
            BlueprintGuids.WeaponPlus1,
            BlueprintGuids.WeaponPlus2,
            BlueprintGuids.WeaponPlus3,
            BlueprintGuids.WeaponPlus4,
            BlueprintGuids.WeaponPlus5,
            BlueprintGuids.WeaponPlus6,
        };

        public static bool TryDecorateName(ItemEntity item, ref string name)
        {
            if (item == null || !item.IsIdentified || string.IsNullOrEmpty(name))
                return false;

            var enchBps = item.Enchantments?
                .Select(e => e?.Blueprint)
                .Where(bp => bp != null)
                .ToList();
            if (enchBps == null || enchBps.Count == 0) return false;

            // ¿hay al menos un RRE (por el componente de precio)?
            bool hasRRE = enchBps.Any(bp => bp.GetComponent<RRE_PriceDeltaComponent>() != null);
            if (!hasRRE) return false;

            // Prefijos: materiales + principal RRE
            var prefixes = new List<string>();
            foreach (var id in MaterialIds)
            {
                var mat = enchBps.FirstOrDefault(bp =>
                    bp.AssetGuid.ToString().Equals(id, StringComparison.OrdinalIgnoreCase));
                if (mat == null) continue;

                var text = GetPrefixLikeText(mat);
                if (!string.IsNullOrWhiteSpace(text)) prefixes.Add(text);
            }

            var mainEnchant = enchBps
                .Where(bp => (bp.name?.StartsWith("RRE_", StringComparison.OrdinalIgnoreCase) ?? false)
                             && bp.GetComponent<RRE_PriceDeltaComponent>() == null)
                .OrderByDescending(bp => bp.EnchantmentCost)
                .ThenBy(bp => bp.AssetGuid.ToString(), StringComparer.Ordinal)
                .FirstOrDefault();
            var mainPrefix = GetPrefixLikeText(mainEnchant);
            if (!string.IsNullOrWhiteSpace(mainPrefix)) prefixes.Add(mainPrefix);

            var finalPrefix = prefixes.Count > 0 ? string.Join(" ", prefixes).Trim() : null;
            if (!string.IsNullOrWhiteSpace(finalPrefix) &&
                !name.StartsWith(finalPrefix + " ", StringComparison.OrdinalIgnoreCase))
            {
                name = $"{finalPrefix} {name}";
            }

            // Sufijo: enhancement (solo uno)
            BlueprintItemEnchantment weaponEnh = null;
            foreach (var id in WeaponEnhancementIds)
            {
                weaponEnh = enchBps.FirstOrDefault(bp =>
                    bp.AssetGuid.ToString().Equals(id, StringComparison.OrdinalIgnoreCase));
                if (weaponEnh != null) break;
            }

            var suffix = GetSuffixLikeText(weaponEnh);
            if (!string.IsNullOrWhiteSpace(suffix) &&
                name.IndexOf(" " + suffix, StringComparison.OrdinalIgnoreCase) < 0)
            {
                name = $"{name} {suffix}";
            }

            return true;
        }

        private static string GetPrefixLikeText(BlueprintItemEnchantment bp)
        {
            if (bp == null) return null;

            if (!string.IsNullOrWhiteSpace(bp.Prefix))
                return bp.Prefix.Trim();

            if (bp is BlueprintWeaponEnchantment bwe)
            {
                var t = bwe.m_EnchantName?.ToString();
                if (!string.IsNullOrWhiteSpace(t)) return t.Trim();
            }
            return bp.name?.Trim();
        }

        private static string GetSuffixLikeText(BlueprintItemEnchantment bp)
        {
            if (bp == null) return null;

            if (!string.IsNullOrWhiteSpace(bp.Suffix))
                return bp.Suffix.Trim();

            var cost = bp.EnchantmentCost;
            if (cost > 0 && (bp.name?.StartsWith("Enhancement", StringComparison.OrdinalIgnoreCase) ?? false))
                return $"+{cost}";

            return null;
        }
    }
}
