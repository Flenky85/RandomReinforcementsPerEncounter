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

    public static bool TryUseBaseName(ItemEntity item, ref string name)
    {
        if (item == null || !item.IsIdentified)
            return false;

        var enchBps = item.Enchantments?
            .Select(e => e?.Blueprint)
            .Where(bp => bp != null)
            .ToList();

        if (enchBps == null || enchBps.Count == 0)
            return false;

        bool isRREMagicItem =
            enchBps.Any(bp =>
                bp.GetComponent<RRE_PriceDeltaComponent>() != null);

        if (!isRREMagicItem)
            return false;

        var baseName = item.Blueprint?.Name;

        if (string.IsNullOrWhiteSpace(baseName))
            return false;

        if (name.Equals(baseName, StringComparison.Ordinal))
            return false;

        name = baseName;
        return true;
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

        bool isRREMagicItem =
            enchBps.Any(bp =>
                bp.GetComponent<RRE_PriceDeltaComponent>() != null);

        if (!isRREMagicItem)
            return false;

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

        var originalName = name;

        var allPrefixes = enchBps
            .Where(bp =>
                !IsMaterial(bp) &&
                !IsEnhancement(bp) &&
                !string.IsNullOrWhiteSpace(bp.Prefix))
            .Select(bp => bp.Prefix.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(text => text.Length)
            .ToList();

        var allSuffixes = enchBps
            .Where(bp =>
                !IsMaterial(bp) &&
                !IsEnhancement(bp) &&
                !string.IsNullOrWhiteSpace(bp.Suffix))
            .Select(bp => SanitizeSuffix(bp.Suffix))
            .Where(text => !string.IsNullOrWhiteSpace(text))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(text => text.Length)
            .ToList();

        var workingName = name.Trim();

        workingName = StripLeadingTokens(workingName, allPrefixes);

        var (_, baseName) = SplitLeadingMaterials(workingName, enchBps);

        baseName = StripLeadingTokens(baseName, allPrefixes);

        baseName = StripTrailingTokens(baseName, allSuffixes);

        baseName = Regex.Replace(
            baseName,
            @"\s+\+\d+\s*$",
            "",
            RegexOptions.IgnoreCase
        ).Trim();

        baseName = StripTrailingTokens(baseName, allSuffixes);

        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(materialLeadWanted))
            parts.Add(materialLeadWanted);

        if (!string.IsNullOrWhiteSpace(prefixText))
            parts.Add(prefixText);

        if (!string.IsNullOrWhiteSpace(baseName))
            parts.Add(baseName);

        if (!string.IsNullOrWhiteSpace(plusN))
            parts.Add(plusN);

        if (!string.IsNullOrWhiteSpace(suffixText))
            parts.Add(suffixText);

        name = string.Join(" ", parts);

        return !name.Equals(originalName, StringComparison.Ordinal);
    }

    private static string StripLeadingTokens(
        string text,
        IEnumerable<string> tokens)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var result = text.Trim();

        while (true)
        {
            var matchedToken = tokens.FirstOrDefault(token =>
                !string.IsNullOrWhiteSpace(token) &&
                (result.Equals(token, StringComparison.OrdinalIgnoreCase) ||
                 result.StartsWith(token + " ", StringComparison.OrdinalIgnoreCase)));

            if (matchedToken == null)
                break;

            result = result.Substring(matchedToken.Length).TrimStart();
        }

        return result;
    }

    private static string StripTrailingTokens(
        string text,
        IEnumerable<string> tokens)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var result = text.Trim();

        while (true)
        {
            var matchedToken = tokens.FirstOrDefault(token =>
                !string.IsNullOrWhiteSpace(token) &&
                (result.Equals(token, StringComparison.OrdinalIgnoreCase) ||
                 result.EndsWith(" " + token, StringComparison.OrdinalIgnoreCase)));

            if (matchedToken == null)
                break;

            result = result.Substring(
                0,
                result.Length - matchedToken.Length
            ).TrimEnd();
        }

        return result;
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
