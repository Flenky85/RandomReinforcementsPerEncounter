using BlueprintCore.Utils;
using Kingmaker.Localization;
using System.Text;

namespace RandomReinforcementsPerEncounter.Config.Localization
{
    internal enum ArtifactKind { Enchant, Feature }

    internal static class KeyBuilder
    {
        private const string ModPrefix = "RRE.";

        /// Genera keys de localización + bpName y devuelve las LocalizedString listas (sin auto-links).
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
