using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Items;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
        BlueprintGuids.WeaponPlus6,
        BlueprintGuids.WeaponPlus5,
        BlueprintGuids.WeaponPlus4,
        BlueprintGuids.WeaponPlus3,
        BlueprintGuids.WeaponPlus2,
        BlueprintGuids.WeaponPlus1,
    };

    private static readonly HashSet<string> EnhancementIdsSet =
        new HashSet<string>(WeaponEnhancementIds, StringComparer.OrdinalIgnoreCase);

    private static bool IsEnhancement(BlueprintItemEnchantment bp)
        => bp != null && EnhancementIdsSet.Contains(bp.AssetGuid.ToString());

    private static bool IsPurePlusSuffix(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return false;
        return System.Text.RegularExpressions.Regex.IsMatch(s.Trim(), @"^\+\d+$");
    }

    private static string SanitizeSuffix(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        var t = s.Trim();

        if (System.Text.RegularExpressions.Regex.IsMatch(t, @"^\+\d+$")) return null;

        t = System.Text.RegularExpressions.Regex.Replace(t, @"^\+\d+\s+", "");
        return string.IsNullOrWhiteSpace(t) ? null : t;
    }

    private static string BuildMaterialLead(List<BlueprintItemEnchantment> enchBps)
    {
        var mats = enchBps
            .Where(IsMaterial)
            .Select(GetPrefixLikeText)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        return string.Join(" ", mats);
    }

    public static bool TryDecorateName(ItemEntity item, ref string name)
    {
        if (item == null || !item.IsIdentified || string.IsNullOrEmpty(name))
            return false;

        var enchBps = item.Enchantments?
            .Select(e => e?.Blueprint)
            .Where(bp => bp != null)
            .ToList();
        if (enchBps == null || enchBps.Count == 0) return false;

        bool hasRREMagic =
            enchBps.Any(bp => (bp.name?.StartsWith("RRE_", StringComparison.OrdinalIgnoreCase) ?? false)) ||
            enchBps.Any(bp => bp.GetComponent<RRE_PriceDeltaComponent>() != null) ||
            enchBps.Any(bp => !string.IsNullOrWhiteSpace(bp.Prefix) || !string.IsNullOrWhiteSpace(bp.Suffix));

        bool hasInteresting = hasRREMagic || enchBps.Any(IsMaterial);
        if (!hasInteresting) return false;

        var bestPrefixBp = enchBps
            .Where(bp => !string.IsNullOrWhiteSpace(bp.Prefix) && !IsMaterial(bp) && !IsEnhancement(bp))
            .OrderByDescending(bp => bp.EnchantmentCost)
            .ThenBy(bp => bp.AssetGuid.ToString(), StringComparer.Ordinal)
            .FirstOrDefault();
        var prefixText = bestPrefixBp?.Prefix?.Trim();

        BlueprintItemEnchantment bestEnh = null;
        foreach (var id in WeaponEnhancementIds)
        {
            bestEnh = enchBps.FirstOrDefault(bp =>
                bp.AssetGuid.ToString().Equals(id, StringComparison.OrdinalIgnoreCase));
            if (bestEnh != null) break;
        }
        var plusN = GetEnhancementPlus(bestEnh);

        var bestSuffixBp = enchBps
            .Where(bp => !string.IsNullOrWhiteSpace(bp.Suffix) && !IsMaterial(bp) && !IsEnhancement(bp) && !IsPurePlusSuffix(bp.Suffix))
            .OrderByDescending(bp => bp.EnchantmentCost)
            .ThenBy(bp => bp.AssetGuid.ToString(), StringComparer.Ordinal)
            .FirstOrDefault();
        var suffixText = SanitizeSuffix(bestSuffixBp?.Suffix);

        var materialLeadWanted = BuildMaterialLead(enchBps);

        if (string.IsNullOrWhiteSpace(prefixText) &&
            string.IsNullOrWhiteSpace(plusN) &&
            string.IsNullOrWhiteSpace(suffixText) &&
            string.IsNullOrWhiteSpace(materialLeadWanted))
            return false;

        bool changed = false;

        if (!string.IsNullOrWhiteSpace(prefixText) &&
            name.StartsWith(prefixText + " ", StringComparison.OrdinalIgnoreCase))
        {
            name = name.Substring(prefixText.Length + 1);
            changed = true;
        }

        var (materialLeadCurrent, rest) = SplitLeadingMaterials(name, enchBps);

        if (!string.IsNullOrWhiteSpace(prefixText) &&
            !rest.StartsWith(prefixText + " ", StringComparison.OrdinalIgnoreCase))
        {
            rest = $"{prefixText} {rest}";
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(materialLeadWanted))
        {
            if (!materialLeadWanted.Equals(materialLeadCurrent, StringComparison.OrdinalIgnoreCase))
                changed = true;

            name = $"{materialLeadWanted} {rest}".Trim();
        }
        else
        {
            name = rest;
        }

        if (EndsWithToken(name, suffixText))
        {
            name = TrimEndToken(name, suffixText);
            changed = true;
        }

        var newName = System.Text.RegularExpressions.Regex.Replace(
            name, @"\s\+\d(?:\s+.+)?$", "", RegexOptions.IgnoreCase);
        if (!ReferenceEquals(newName, name))
        {
            name = newName;
            changed = true;
        }

        string tail = null;
        if (!string.IsNullOrWhiteSpace(plusN)) tail = plusN;
        if (!string.IsNullOrWhiteSpace(suffixText))
            tail = string.IsNullOrWhiteSpace(tail) ? suffixText : $"{tail} {suffixText}";

        if (!string.IsNullOrWhiteSpace(tail) &&
            !name.EndsWith(" " + tail, StringComparison.OrdinalIgnoreCase))
        {
            name = $"{name} {tail}";
            changed = true;
        }

        return changed;
    }

    private static bool IsMaterial(BlueprintItemEnchantment bp)
        => MaterialIds.Any(id => bp.AssetGuid.ToString().Equals(id, StringComparison.OrdinalIgnoreCase));

    private static (string leading, string rest) SplitLeadingMaterials(string currentName, List<BlueprintItemEnchantment> enchBps)
    {
        var materialTexts = enchBps
            .Where(IsMaterial)
            .Select(GetPrefixLikeText)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (materialTexts.Count == 0) return (string.Empty, currentName);

        string leading = string.Empty;
        string rest = currentName;

        for (int i = 0; i < materialTexts.Count; i++)
        {
            bool matched = false;
            foreach (var mt in materialTexts)
            {
                if (rest.StartsWith(mt + " ", StringComparison.OrdinalIgnoreCase))
                {
                    leading = string.IsNullOrEmpty(leading) ? mt : (leading + " " + mt);
                    rest = rest.Substring(mt.Length + 1);
                    matched = true;
                    break;
                }
            }
            if (!matched) break;
        }
        return (leading, rest);
    }

    private static int EnhancementRankFromId(string id)
    {
        for (int i = 0; i < WeaponEnhancementIds.Length; i++)
            if (string.Equals(WeaponEnhancementIds[i], id, StringComparison.OrdinalIgnoreCase))
                return WeaponEnhancementIds.Length - i; 
        return 0;
    }

    private static string GetEnhancementPlus(BlueprintItemEnchantment bp)
    {
        if (bp == null) return null;
        int rank = EnhancementRankFromId(bp.AssetGuid.ToString()); 
        return rank > 0 ? $"+{rank}" : null;
    }

    private static bool EndsWithToken(string s, string token)
    {
        if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(token)) return false;
        return s.TrimEnd().EndsWith(token, StringComparison.OrdinalIgnoreCase);
    }

    private static string TrimEndToken(string s, string token)
    {
        if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(token)) return s;
        var trimmed = s.TrimEnd();
        if (trimmed.EndsWith(token, StringComparison.OrdinalIgnoreCase))
        {
            var keep = trimmed.Substring(0, trimmed.Length - token.Length).TrimEnd();
            return keep;
        }
        return s;
    }

    private static string GetPrefixLikeText(BlueprintItemEnchantment bp)
    {
        if (bp == null) return null;
        if (!string.IsNullOrWhiteSpace(bp.Prefix)) return bp.Prefix.Trim();
        if (bp is BlueprintWeaponEnchantment bwe)
        {
            var t = bwe.m_EnchantName?.ToString();
            if (!string.IsNullOrWhiteSpace(t)) return t.Trim();
        }
        return bp.name?.Trim();
    }
}
