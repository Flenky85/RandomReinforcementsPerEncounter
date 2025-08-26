using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityModManagerNet;
using Kingmaker.Blueprints.Items.Ecnchantments;
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.GameApi.Loot;

namespace RandomReinforcementsPerEncounter.UI.Pages
{
    internal static class EnchantEncyclopediaPage
    {
        private const float MaxWidth = 920f;
        private const float MidWidth = 860f;
        private const float TextWidth = 820f;

        private static GUIStyle _bold, _wrap, _pill, _center;

        private static int _gripIndex = 0;
        private static readonly string[] _gripTabs = { "One Handed", "Two Handed", "Double" };

        private static EnchantType _selectedFamily = EnchantType.OnHit;
        private static int _selectedTypeIndex = 0;
               
        private struct IdMeta { public WeaponGrip hand; public bool applyBothOnDouble; }
        private static readonly Dictionary<string, IdMeta> _idMeta =
            new Dictionary<string, IdMeta>(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, int> _idToTier =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, EnchantData> _idToData =
            new Dictionary<string, EnchantData>(StringComparer.OrdinalIgnoreCase);

        private static bool _metaBuilt = false;
        private static bool _indexBuilt = false;

        private class TypeGroup
        {
            public string Label;                           
            public HashSet<string> Ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        private struct TierResult { public int Tier; public BlueprintItemEnchantment Bp; }

        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            EnsureStyles();
            EnsureMetaCache();   
            EnsureIdIndex();     

            GUILayout.BeginVertical(GUILayout.Width(MaxWidth));
            
            GUILayout.Label("Weapons", _bold);
            _gripIndex = GUILayout.Toolbar(_gripIndex, _gripTabs, GUILayout.Width(MaxWidth));
            WeaponGrip grip = (_gripIndex == 1) ? WeaponGrip.TwoHanded : (_gripIndex == 2) ? WeaponGrip.Double : WeaponGrip.OneHanded;

            GUILayout.Space(8);

            EnchantType[] families = GetFamiliesForGrip(grip).ToArray();
            if (families.Length == 0)
            {
                GUILayout.Label("No enchant families available for this grip.", _bold);
                GUILayout.EndVertical();
                return;
            }
            if (Array.IndexOf(families, _selectedFamily) < 0) _selectedFamily = families[0];

            GUILayout.Label("Families", _bold);
            {
                string[] famLabels = families.Select(FriendlyFamilyName).ToArray();
                int selIdx = Array.IndexOf(families, _selectedFamily);
                DrawTypeBar(famLabels, selIdx, delegate (int idx) { _selectedFamily = families[idx]; _selectedTypeIndex = 0; });
            }
            GUILayout.Space(8);

            List<TypeGroup> groups = new List<TypeGroup>(BuildTypeGroupsFromPools(grip, _selectedFamily));
            if (groups.Count == 0)
            {
                GUILayout.Label("No enchant types in this family for this grip.", _bold);
                GUILayout.EndVertical();
                return;
            }
            if (_selectedTypeIndex < 0 || _selectedTypeIndex >= groups.Count) _selectedTypeIndex = 0;

            GUILayout.Label("Types", _bold);
            {
                string[] typeLabels = groups.Select(g => g.Label).ToArray();
                DrawGrid(typeLabels, _selectedTypeIndex, 4, delegate (int idx) { _selectedTypeIndex = idx; }, MidWidth);
            }
            GUILayout.Space(8);

            TypeGroup chosen = groups[_selectedTypeIndex];
            GUILayout.Label("Tiers", _bold);

            foreach (TierResult tr in GetTierBlueprintsForGroup(chosen, grip))
            {
                if (tr.Bp == null) continue;
                GUILayout.BeginVertical("box", GUILayout.Width(MidWidth));
                GUILayout.Label(TryGetEnchantName(tr.Bp), _bold);
                GUILayout.Label(TryGetEnchantDescription(tr.Bp), _wrap, GUILayout.Width(TextWidth));
                GUILayout.EndVertical();
                GUILayout.Space(6);
            }

            GUILayout.EndVertical();
        }

        
        private static IEnumerable<EnchantType> GetFamiliesForGrip(WeaponGrip grip)
        {
            HashSet<EnchantType> set = new HashSet<EnchantType>();
            for (int t = 1; t <= 6; t++)
            {
                foreach (AffixKind affix in new[] { AffixKind.Prefix, AffixKind.Suffix })
                {
                    foreach (var c in LootBuckets.GetCandidatesByTierAndAffix(t, affix))
                    {
                        if (string.IsNullOrWhiteSpace(c.id)) continue;
                        if (!HasGripMatch(c.id, grip)) continue;

                        EnchantType fam;
                        if (TryGetFamilyById(c.id, out fam)) set.Add(fam);
                    }
                }
            }
            List<EnchantType> list = new List<EnchantType>(set);
            list.Sort((a, b) => string.Compare(FriendlyFamilyName(a), FriendlyFamilyName(b), StringComparison.OrdinalIgnoreCase));
            return list;
        }

        private static IEnumerable<TypeGroup> BuildTypeGroupsFromPools(WeaponGrip grip, EnchantType fam)
        {
            Dictionary<string, TypeGroup> byLabel = new Dictionary<string, TypeGroup>(StringComparer.OrdinalIgnoreCase);

            for (int t = 1; t <= 6; t++)
            {
                foreach (AffixKind affix in new[] { AffixKind.Prefix, AffixKind.Suffix })
                {
                    foreach (var c in LootBuckets.GetCandidatesByTierAndAffix(t, affix))
                    {
                        if (string.IsNullOrWhiteSpace(c.id)) continue;
                        if (!HasGripMatch(c.id, grip)) continue;

                        EnchantType foundFam;
                        if (!TryGetFamilyById(c.id, out foundFam)) continue;
                        if (foundFam != fam) continue;

                        var bp = LootUtils.TryLoadEnchant(c.id);
                        string label = BuildBaseLabel(bp); 
                        if (string.IsNullOrWhiteSpace(label)) label = "(Unnamed) <" + ShortId(c.id) + ">";

                        TypeGroup g;
                        if (!byLabel.TryGetValue(label, out g))
                        {
                            g = new TypeGroup { Label = label };
                            byLabel[label] = g;
                        }
                        g.Ids.Add(c.id);
                                               
                        if (!_idToTier.ContainsKey(c.id))
                        {
                            int parsedTier;
                            if (TryParseTierFromName(bp, out parsedTier))
                                _idToTier[c.id] = parsedTier;
                        }
                    }
                }
            }

            List<TypeGroup> result = new List<TypeGroup>(byLabel.Values);
            result.Sort((a, b) => string.Compare(a.Label, b.Label, StringComparison.OrdinalIgnoreCase));
            return result;
        }

        private static IEnumerable<TierResult> GetTierBlueprintsForGroup(TypeGroup group, WeaponGrip grip)
        {
            for (int tier = 1; tier <= 6; tier++)
            {
                List<string> candidates = new List<string>();
                foreach (var id in group.Ids)
                {
                    int t;
                    if (_idToTier.TryGetValue(id, out t) && t == tier && HasGripMatch(id, grip))
                        candidates.Add(id);
                }
                if (candidates.Count == 0) continue;

                string pickedId = ChooseBestIdForGrip(candidates, grip);
                var bp = LootUtils.TryLoadEnchant(pickedId);
                if (bp != null) yield return new TierResult { Tier = tier, Bp = bp };
            }
        }

        private static bool TryGetFamilyById(string id, out EnchantType fam)
        {
            EnchantData d;
            if (_idToData.TryGetValue(id, out d))
            {
                fam = d.Type;
                return true;
            }

            foreach (var x in EnchantList.Item)
            {
                if (x == null) continue;
                if (ArrayContains(x.AssetIDT1, id) || ArrayContains(x.AssetIDT2, id) ||
                    ArrayContains(x.AssetIDT3, id) || ArrayContains(x.AssetIDT4, id) ||
                    ArrayContains(x.AssetIDT5, id) || ArrayContains(x.AssetIDT6, id))
                {
                    fam = x.Type;
                    return true;
                }
            }
            fam = EnchantType.Others;
            return false;
        }

        private static bool ArrayContains(string[] arr, string id)
        {
            if (arr == null) return false;
            for (int i = 0; i < arr.Length; i++) if (string.Equals(arr[i], id, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static void EnsureMetaCache()
        {
            if (_metaBuilt) return;
            for (int t = 1; t <= 6; t++)
            {
                foreach (AffixKind affix in new[] { AffixKind.Prefix, AffixKind.Suffix })
                {
                    foreach (var c in LootBuckets.GetCandidatesByTierAndAffix(t, affix))
                    {
                        if (string.IsNullOrWhiteSpace(c.id)) continue;
                        if (!_idMeta.ContainsKey(c.id))
                        {
                            IdMeta meta = new IdMeta();
                            meta.hand = c.hand;
                            meta.applyBothOnDouble = c.applyBothOnDouble; 
                            _idMeta[c.id] = meta;
                        }
                    }
                }
            }
            _metaBuilt = true;
        }

        private static void EnsureIdIndex()
        {
            if (_indexBuilt) return;
            
            foreach (var d in EnchantList.Item)
            {
                if (d == null) continue;

                IndexTierArray(d, d.AssetIDT1, 1);
                IndexTierArray(d, d.AssetIDT2, 2);
                IndexTierArray(d, d.AssetIDT3, 3);
                IndexTierArray(d, d.AssetIDT4, 4);
                IndexTierArray(d, d.AssetIDT5, 5);
                IndexTierArray(d, d.AssetIDT6, 6);
            }

            _indexBuilt = true;
        }

        private static void IndexTierArray(EnchantData d, string[] arr, int tier)
        {
            if (arr == null) return;
            for (int k = 0; k < arr.Length; k++)
            {
                string id = arr[k];
                if (string.IsNullOrWhiteSpace(id)) continue;
                if (!_idToTier.ContainsKey(id)) _idToTier[id] = tier;
                if (!_idToData.ContainsKey(id)) _idToData[id] = d;
            }
        }

        private static bool HasGripMatch(string id, WeaponGrip grip)
        {
            IdMeta meta;
            if (!_idMeta.TryGetValue(id, out meta)) return true; 
            if (grip == WeaponGrip.OneHanded) return meta.hand == WeaponGrip.OneHanded;
            if (grip == WeaponGrip.TwoHanded) return meta.hand == WeaponGrip.TwoHanded;
            return meta.hand == WeaponGrip.TwoHanded || (meta.hand == WeaponGrip.OneHanded && meta.applyBothOnDouble);
        }

        private static string ChooseBestIdForGrip(List<string> ids, WeaponGrip grip)
        {
            if (ids == null || ids.Count == 0) return null;

            if (grip == WeaponGrip.Double)
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    IdMeta m; if (_idMeta.TryGetValue(ids[i], out m) && m.hand == WeaponGrip.OneHanded && m.applyBothOnDouble) return ids[i];
                }
                for (int i = 0; i < ids.Count; i++)
                {
                    IdMeta m; if (_idMeta.TryGetValue(ids[i], out m) && m.hand == WeaponGrip.TwoHanded) return ids[i];
                }
                for (int i = 0; i < ids.Count; i++)
                {
                    IdMeta m; if (_idMeta.TryGetValue(ids[i], out m) && m.hand == WeaponGrip.OneHanded) return ids[i];
                }
                return ids[0];
            }
            else if (grip == WeaponGrip.OneHanded)
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    IdMeta m; if (_idMeta.TryGetValue(ids[i], out m) && m.hand == WeaponGrip.OneHanded) return ids[i];
                }
            }
            else 
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    IdMeta m; if (_idMeta.TryGetValue(ids[i], out m) && m.hand == WeaponGrip.TwoHanded) return ids[i];
                }
            }
            return ids[0];
        }

        private static string BuildBaseLabel(BlueprintItemEnchantment bp)
        {
            string n = TryGetEnchantName(bp);
            if (string.IsNullOrWhiteSpace(n)) return null;
            var m = Regex.Match(n, @"\s*\(T(\d)\)\s*$");
            if (m.Success) return n.Substring(0, m.Index).Trim();
            return n.Trim();
        }

        private static bool TryParseTierFromName(BlueprintItemEnchantment bp, out int tier)
        {
            tier = 0;
            if (bp == null) return false;
            string n = TryGetEnchantName(bp);
            if (string.IsNullOrWhiteSpace(n)) return false;
            var m = Regex.Match(n, @"\(T(\d)\)");
            if (!m.Success) return false;
            int t;
            if (int.TryParse(m.Groups[1].Value, out t) && t >= 1 && t <= 6) { tier = t; return true; }
            return false;
        }

        private static void EnsureStyles()
        {
            if (_bold != null) return;
            _bold = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
            _wrap = new GUIStyle(GUI.skin.label) { wordWrap = true };
            _center = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            _pill = new GUIStyle(GUI.skin.button) { fontSize = 12, padding = new RectOffset(10, 10, 6, 6), margin = new RectOffset(4, 4, 2, 2) };
        }

        private static void DrawTypeBar(string[] labels, int selected, Action<int> onPick)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(MidWidth));
            for (int i = 0; i < labels.Length; i++)
            {
                bool active = (i == selected);
                Color prev = GUI.color;
                if (active) GUI.color = new Color(0.85f, 0.85f, 1f);
                if (GUILayout.Button(labels[i], _pill)) onPick(i);
                GUI.color = prev;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void DrawGrid(string[] labels, int selected, int newCols, Action<int> onPick, float width)
        {
            int cols = Mathf.Max(1, newCols);
            int rows = Mathf.CeilToInt(labels.Length / (float)cols);
            int idx = 0;
            for (int r = 0; r < rows; r++)
            {
                GUILayout.BeginHorizontal(GUILayout.Width(width));
                for (int c = 0; c < cols; c++)
                {
                    if (idx >= labels.Length) { GUILayout.FlexibleSpace(); break; }
                    bool active = (idx == selected);
                    Color prev = GUI.color;
                    if (active) GUI.color = new Color(0.85f, 0.85f, 1f);
                    if (GUILayout.Button(labels[idx], _pill, GUILayout.Width(width / cols - 8))) onPick(idx);
                    GUI.color = prev;
                    idx++;
                }
                GUILayout.EndHorizontal();
            }
        }

        private static string FriendlyFamilyName(EnchantType t)
        {
            var tokens = TokenizeEnumName(t.ToString());
            if (tokens.Count == 0) return t.ToString();

            for (int i = 0; i < tokens.Count; i++)
            {
                var w = tokens[i];
                if (i == 0)
                    tokens[i] = IsAcronym(w) ? w : Capitalize(w);   
                else
                    tokens[i] = IsAcronym(w) ? w : w.ToLowerInvariant(); 
            }
            return string.Join(" ", tokens);
        }

        private static List<string> TokenizeEnumName(string name)
        {
            name = name.Replace('_', ' ');
            var tokens = new List<string>();
            var parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var rx = new System.Text.RegularExpressions.Regex(
                @"[A-Z]{2,}(?=$|[A-Z][a-z])|[A-Z]?[a-z]+|\d+",
                System.Text.RegularExpressions.RegexOptions.Compiled);

            for (int i = 0; i < parts.Length; i++)
            {
                var m = rx.Matches(parts[i]);
                for (int j = 0; j < m.Count; j++)
                    tokens.Add(m[j].Value);
            }
            return tokens;
        }

        private static bool IsAcronym(string w)
        {
            bool hasLetter = false;
            for (int i = 0; i < w.Length; i++)
            {
                char ch = w[i];
                if (char.IsLetter(ch))
                {
                    hasLetter = true;
                    if (!char.IsUpper(ch)) return false;
                }
            }
            return hasLetter && w.Length >= 2;
        }

        private static string Capitalize(string w)
        {
            if (string.IsNullOrEmpty(w)) return w;
            if (w.Length == 1) return w.ToUpperInvariant();
            return char.ToUpperInvariant(w[0]) + w.Substring(1).ToLowerInvariant();
        }

        private static string TryGetEnchantName(BlueprintItemEnchantment bp)
        {
            try { var s = (bp != null && bp.m_EnchantName != null) ? bp.m_EnchantName.ToString() : null; if (!string.IsNullOrWhiteSpace(s)) return s; }
            catch { }
            return (bp != null && !string.IsNullOrWhiteSpace(bp.name)) ? bp.name : "(Unnamed enchant)";
        }

        private static string TryGetEnchantDescription(BlueprintItemEnchantment bp)
        {
            try
            {
                var s = (bp != null && bp.Description != null) ? bp.Description.ToString() : null;
                if (!string.IsNullOrWhiteSpace(s))
                    return SanitizeDescription(s);
            }
            catch { }
            return "";
        }

        private static string SanitizeDescription(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";

            string text = s;

            text = System.Text.RegularExpressions.Regex.Replace(
                text, @"<br\s*/?>", "\n",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            text = System.Text.RegularExpressions.Regex.Replace(
                text, @"<link[^>]*>(.*?)</link>", "$1",
                System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            text = System.Text.RegularExpressions.Regex.Replace(
                text, @"<(b|i|u|color|size|indent|nobr)[^>]*>(.*?)</\1>",
                "$2",
                System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            text = System.Text.RegularExpressions.Regex.Replace(
                text, @"<[^>]+>", "",
                System.Text.RegularExpressions.RegexOptions.Singleline);

            text = System.Net.WebUtility.HtmlDecode(text);

            text = text.Replace("\r\n", "\n").Replace("\r", "\n");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"[ \t]+\n", "\n");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\n{3,}", "\n\n");

            return text.Trim();
        }

        private static string ShortId(string id)
        {
            if (string.IsNullOrEmpty(id)) return "null";
            if (id.Length <= 8) return id;
            return id.Substring(0, 8);
        }
    }
}
