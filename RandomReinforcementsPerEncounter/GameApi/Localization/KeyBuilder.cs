using BlueprintCore.Utils;
using Kingmaker.Localization;
using System.Text;

namespace RandomReinforcementsPerEncounter.GameApi.Localization
{
    internal enum ArtifactKind
    {
        Enchant,
        Feature,
        // (futuro?) Ability, Buff, Item, Etc
    }

    internal static class KeyBuilder
    {
        private const string ModPrefix = "RRE.";

        /// <summary>
        /// Genera keys de localización + nombre técnico del blueprint (bpName) con sufijo por tipo (Enchant/Feature).
        /// </summary>
        internal static (string nameKey, string descKey, string bpName, LocalizedString locName, LocalizedString locAffix)
        BuildTierKeys(string nameRoot, int tierIndex, string name, ArtifactKind kind, string affixName)
        {
            string nameKey = $"{ModPrefix}{nameRoot}.T{tierIndex}.Name";
            string affixKey = $"{ModPrefix}{nameRoot}.T{tierIndex}.Affix";
            string descKey = $"{ModPrefix}{nameRoot}.T{tierIndex}.Desc";

            // Sufijo según tipo
            string suffix = SuffixOf(kind);
            // Sanitiza nameRoot para usarlo en nombres de blueprint (sin espacios/raros)
            string safeRoot = SanitizeForBpName(nameRoot);

            string bpName = $"RRE_{safeRoot}_T{tierIndex}_{suffix}";

            var locName = LocalizationTool.CreateString(nameKey, $"{name} (T{tierIndex})");
    
            var locAffix = LocalizationTool.CreateString(affixKey, $"{affixName}");
            
            return (nameKey, descKey, bpName, locName, locAffix);
        }

        private static string SuffixOf(ArtifactKind kind)
        {
            switch (kind)
            {
                case ArtifactKind.Feature: return "Feature";
                case ArtifactKind.Enchant:
                default: return "Enchant";
            }
        }

        // Sustituye caracteres no alfanuméricos por '_'
        private static string SanitizeForBpName(string s)
        {
            var sb = new StringBuilder(s.Length);
            foreach (char c in s)
                sb.Append(char.IsLetterOrDigit(c) ? c : '_');
            return sb.ToString().Trim('_');
        }
    }
}
