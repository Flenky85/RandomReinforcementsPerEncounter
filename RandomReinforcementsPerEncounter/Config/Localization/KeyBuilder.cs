using BlueprintCore.Utils;
using Kingmaker.Localization;
using System.Text;

namespace RandomReinforcementsPerEncounter.Config.Localization
{
    internal enum ArtifactKind { Enchant, Feature }

    /// <summary>
    /// Builds stable localization keys and a sanitized Blueprint name for tiered artifacts.
    /// 
    /// ⚠ SAVE-COMPAT WARNING:
    /// - Treat the generated keys (name/affix/desc) and the blueprint name as *stable IDs*.
    /// - Changing the format, prefix, or inputs (e.g., ModPrefix, nameRoot, tierIndex, suffix)
    ///   after releasing a version can break existing saves or produce missing-text/missing-asset issues.
    /// - If you must change them, provide migration/shims rather than renaming.
    /// </summary>
    internal static class KeyBuilder
    {
        private const string ModPrefix = "RRE."; // Changing this later will invalidate all existing keys.

        /// <summary>
        /// Creates: 
        /// - deterministic localization keys for Name/Affix/Desc (RRE.{root}.T{n}.Name|Affix|Desc),
        /// - a sanitized, tiered Blueprint name (RRE_{root}_T{n}_{suffix}),
        /// - and the corresponding LocalizedString entries via BlueprintCore.
        ///
        /// NOTE: We return nameKey and descKey; affixKey is created but not returned by design.
        /// If you need the affix key string elsewhere, extend the tuple accordingly.
        /// </summary>
        internal static (string nameKey, string descKey, string bpName,
                         LocalizedString locName, LocalizedString locAffix, LocalizedString locDesc)
        BuildTierKeys(string nameRoot, int tierIndex, string name, ArtifactKind kind, string affixName, string descText)
        {
            string nameKey = $"{ModPrefix}{nameRoot}.T{tierIndex}.Name";
            string affixKey = $"{ModPrefix}{nameRoot}.T{tierIndex}.Affix";
            string descKey = $"{ModPrefix}{nameRoot}.T{tierIndex}.Desc";

            string suffix = SuffixOf(kind);                 
            string safeRoot = SanitizeForBpName(nameRoot);  
            string bpName = $"RRE_{safeRoot}_T{tierIndex}_{suffix}";

            var locName = LocalizationTool.CreateString(nameKey, $"{name} (T{tierIndex})", tagEncyclopediaEntries: false);
            var locAffix = LocalizationTool.CreateString(affixKey, $"{affixName}", tagEncyclopediaEntries: false);
            var locDesc = LocalizationTool.CreateString(descKey, descText);

            return (nameKey, descKey, bpName, locName, locAffix, locDesc);
        }

        private static string SuffixOf(ArtifactKind kind) => kind == ArtifactKind.Feature ? "Feature" : "Enchant";

        private static string SanitizeForBpName(string s)
        {
            var sb = new StringBuilder(s.Length);
            foreach (char c in s) sb.Append(char.IsLetterOrDigit(c) ? c : '_');
            return sb.ToString().Trim('_');
        }
    }
}
