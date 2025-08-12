using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments; // en algunas versiones es Kingmaker.Blueprints.Items.Ecnchantments (typo original)
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.Utility;
using UnityEngine;

//como usarlo
//RandomReinforcementsPerEncounter.EnchantInspector.DumpByGuid("633b38ff1d11de64a91d490c683ab1c8");

namespace RandomReinforcementsPerEncounter
{
    public static class EnchantInspector
    {
        // ====== ENTRYPOINTS ======
        public static void DumpByGuid(string guid, int maxDepth = 4, int maxItemsPerCollection = 64)
        {
            if (string.IsNullOrEmpty(guid))
            {
                Debug.LogError("[EnchantInspector] Empty GUID.");
                return;
            }

            var bp = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(guid);
            if (bp == null)
            {
                Debug.LogError("[EnchantInspector] BlueprintWeaponEnchantment not found for GUID: " + guid);
                return;
            }

            DumpBlueprint(bp, maxDepth, maxItemsPerCollection);
        }

        public static void DumpBlueprint(BlueprintWeaponEnchantment enchant, int maxDepth = 4, int maxItemsPerCollection = 64)
        {
            if (enchant == null)
            {
                Debug.LogError("[EnchantInspector] Null enchant.");
                return;
            }

            try
            {
                var sb = new StringBuilder(4096);
                var visited = new HashSet<object>(new ReferenceEqualityComparer());
                sb.AppendLine("==== Enchantment Dump ====");
                sb.AppendLine("Type: " + enchant.GetType().FullName);
                sb.AppendLine("Name: " + SafeName(enchant));
                sb.AppendLine("AssetGuid: " + SafeAssetGuid(enchant));
                sb.AppendLine("DisplayName: " + SafeGetLocField(enchant, new[] { "m_EnchantName", "EnchantName", "m_DisplayName", "DisplayName" }));
                sb.AppendLine("Description: " + SafeGetLocField(enchant, new[] { "m_Description", "Description" }));
                sb.AppendLine("Prefix: " + SafeGetLocField(enchant, new[] { "m_EnchantPrefix", "EnchantPrefix", "m_Prefix", "Prefix" }));
                sb.AppendLine("Suffix: " + SafeGetLocField(enchant, new[] { "m_EnchantSuffix", "EnchantSuffix", "m_Suffix", "Suffix" }));

                sb.AppendLine();

                // Components (si tu versión expone ComponentsArray)
                try
                {
                    var compsProp = enchant.GetType().GetProperty("ComponentsArray", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (compsProp != null)
                    {
                        var comps = compsProp.GetValue(enchant, null) as Array;
                        sb.AppendLine("ComponentsArray (" + (comps != null ? comps.Length.ToString() : "null") + "):");
                        if (comps != null)
                        {
                            for (int i = 0; i < comps.Length; i++)
                            {
                                var c = comps.GetValue(i);
                                sb.AppendLine("  [" + i + "] " + SafeType(c) + "  " + SafeName(c));
                            }
                        }
                        sb.AppendLine();
                    }
                }
                catch { /* ignore */ }

                sb.AppendLine("---- Full Reflection Dump (recursive) ----");
                RecursiveDump(enchant, sb, visited, 0, maxDepth, maxItemsPerCollection);

                var dir = System.IO.Path.Combine(Application.persistentDataPath, "EnchantDumps");
                try { System.IO.Directory.CreateDirectory(dir); } catch { }

                var fileName = "EnchantDump_"
                    + SafeFileName(SafeName(enchant)) + "_"
                    + SafeFilePart(SafeAssetGuid(enchant)) + ".txt";

                var path = System.IO.Path.Combine(dir, fileName);
                System.IO.File.WriteAllText(path, sb.ToString());
                Debug.Log("[EnchantInspector] Dump written to: " + path);
            }
            catch (Exception ex)
            {
                Debug.LogError("[EnchantInspector] Exception: " + ex);
            }
        }

        // ====== CORE DUMPER ======
        private static void RecursiveDump(object obj, StringBuilder sb, HashSet<object> visited, int depth, int maxDepth, int maxItemsPerCollection)
        {
            if (obj == null)
            {
                Indent(sb, depth).AppendLine("null");
                return;
            }

            if (depth > maxDepth)
            {
                Indent(sb, depth).AppendLine("… (max depth reached)");
                return;
            }

            var type = obj.GetType();

            // Evitar ciclos
            if (!type.IsValueType && !(obj is string))
            {
                if (visited.Contains(obj))
                {
                    Indent(sb, depth).AppendLine($"↺ (visited) {SafeType(obj)} {SafeName(obj)}");
                    return;
                }
                visited.Add(obj);
            }

            // Tipos básicos / string
            if (IsLeaf(type))
            {
                Indent(sb, depth).AppendLine(LeafToString(obj));
                return;
            }

            // LocalizedString: mostrar texto y key
            if (obj is LocalizedString ls)
            {
                Indent(sb, depth).AppendLine($"LocalizedString Key='{SafeLocKey(ls)}' Text='{SafeLocalized(ls)}'");
                return;
            }

            // IEnumerable (colecciones)
            if (obj is IEnumerable enumerable && !(obj is BlueprintScriptableObject))
            {
                int i = 0;
                Indent(sb, depth).AppendLine($"{SafeType(obj)} (IEnumerable):");
                foreach (var item in enumerable)
                {
                    if (i >= maxItemsPerCollection)
                    {
                        Indent(sb, depth + 1).AppendLine("… (truncated)");
                        break;
                    }
                    Indent(sb, depth + 1).Append($"[{i}] ");
                    if (IsLeaf(item?.GetType()))
                    {
                        sb.AppendLine(LeafToString(item));
                    }
                    else
                    {
                        sb.AppendLine(SafeType(item) + " " + SafeName(item));
                        RecursiveDump(item, sb, visited, depth + 2, maxDepth, maxItemsPerCollection);
                    }
                    i++;
                }
                return;
            }

            // Objetos: listar campos y propiedades
            Indent(sb, depth).AppendLine($"{SafeType(obj)} {SafeName(obj)} {{");
            try
            {
                // Campos
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                for (int f = 0; f < fields.Length; f++)
                {
                    var fi = fields[f];
                    if (fi.IsLiteral) continue;
                    object val = null;
                    try { val = fi.GetValue(obj); } catch { /* ignore */ }

                    Indent(sb, depth + 1).Append(fi.Name).Append(": ");
                    if (IsLeaf(val?.GetType()))
                    {
                        sb.AppendLine(LeafToString(val));
                    }
                    else
                    {
                        sb.AppendLine(SafeType(val) + " " + SafeName(val));
                        RecursiveDump(val, sb, visited, depth + 2, maxDepth, maxItemsPerCollection);
                    }
                }

                // Propiedades (sin indexadores)
                var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                for (int p = 0; p < props.Length; p++)
                {
                    var pi = props[p];
                    if (!pi.CanRead) continue;
                    if (pi.GetIndexParameters().Length != 0) continue;
                    object val = null;
                    try { val = pi.GetValue(obj, null); } catch { continue; }

                    Indent(sb, depth + 1).Append(pi.Name).Append(": ");
                    if (IsLeaf(val?.GetType()))
                    {
                        sb.AppendLine(LeafToString(val));
                    }
                    else
                    {
                        sb.AppendLine(SafeType(val) + " " + SafeName(val));
                        RecursiveDump(val, sb, visited, depth + 2, maxDepth, maxItemsPerCollection);
                    }
                }
            }
            catch (Exception ex)
            {
                Indent(sb, depth + 1).AppendLine("<<reflection error>> " + ex.Message);
            }
            Indent(sb, depth).AppendLine("}");
        }

        // ====== HELPERS ======
        private static StringBuilder Indent(StringBuilder sb, int depth)
        {
            for (int i = 0; i < depth; i++) sb.Append("  ");
            return sb;
        }

        private static bool IsLeaf(Type t)
        {
            if (t == null) return true;
            if (t.IsPrimitive) return true;
            if (t == typeof(string)) return true;
            if (t == typeof(decimal)) return true;
            if (t == typeof(Guid)) return true;
            if (t.IsEnum) return true;
            if (typeof(UnityEngine.Vector2) == t || typeof(UnityEngine.Vector3) == t || typeof(UnityEngine.Vector4) == t || typeof(Quaternion) == t)
                return true;
            return false;
        }

        private static string LeafToString(object val)
        {
            if (val == null) return "null";
            return Convert.ToString(val);
        }

        private static string SafeType(object o)
        {
            return o == null ? "(null)" : o.GetType().FullName;
        }

        private static string SafeName(object o)
        {
            if (o == null) return "(null)";
            try
            {
                var so = o as ScriptableObject;
                if (so != null) return so.name ?? "(no-name)";
                var field = o.GetType().GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    var v = field.GetValue(o) as string;
                    if (!string.IsNullOrEmpty(v)) return v;
                }
            }
            catch { }
            return "(" + o.GetType().Name + ")";
        }

        private static string SafeAssetGuid(object o)
        {
            if (o == null) return "null";
            try
            {
                var t = o.GetType();
                var names = new[] { "AssetGuid", "m_AssetGuid", "AssetId", "m_AssetId" };
                for (int i = 0; i < names.Length; i++)
                {
                    var pi = t.GetProperty(names[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (pi != null) { var v = pi.GetValue(o, null); if (v != null) return v.ToString(); }
                    var fi = t.GetField(names[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (fi != null) { var v = fi.GetValue(o); if (v != null) return v.ToString(); }
                }
            }
            catch { }
            return "unknown";
        }

        private static string SafeLocalized(LocalizedString ls)
        {
            try
            {
                if (ls == null) return "(null)";
                var text = ls.ToString();
                if (!string.IsNullOrEmpty(text)) return text;
                var key = SafeLocKey(ls);
                return "[key:" + key + "] (empty at runtime)";
            }
            catch { return "(err)"; }
        }

        private static string SafeLocKey(LocalizedString ls)
        {
            if (ls == null) return "null";
            try
            {
                // m_Key is usually private string
                var fi = typeof(LocalizedString).GetField("m_Key", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                var key = fi != null ? fi.GetValue(ls) as string : null;
                return string.IsNullOrEmpty(key) ? "n/a" : key;
            }
            catch { return "n/a"; }
        }

        private static string SafeFileName(string s)
        {
            if (string.IsNullOrEmpty(s)) return "unnamed";
            var invalid = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                var ch = s[i];
                bool bad = false;
                for (int j = 0; j < invalid.Length; j++) if (ch == invalid[j]) { bad = true; break; }
                sb.Append(bad ? '_' : ch);
            }
            return sb.ToString();
        }

        // Referencia por identidad para evitar ciclos
        private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            bool IEqualityComparer<object>.Equals(object x, object y) { return ReferenceEquals(x, y); }
            int IEqualityComparer<object>.GetHashCode(object obj) { return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj); }
        }
        private static string SafeGetLocField(object obj, string[] names)
        {
            if (obj == null) return "(null)";
            var t = obj.GetType();
            // Campos
            for (int i = 0; i < names.Length; i++)
            {
                var fi = t.GetField(names[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fi != null)
                {
                    try
                    {
                        var v = fi.GetValue(obj) as Kingmaker.Localization.LocalizedString;
                        return SafeLocalized(v);
                    }
                    catch { }
                }
            }
            // Propiedades
            for (int i = 0; i < names.Length; i++)
            {
                var pi = t.GetProperty(names[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (pi != null && pi.CanRead && pi.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        var v = pi.GetValue(obj, null) as Kingmaker.Localization.LocalizedString;
                        return SafeLocalized(v);
                    }
                    catch { }
                }
            }
            return "(n/a)";
        }
        private static string SafeFilePart(string s)
        {
            if (string.IsNullOrEmpty(s)) return "unnamed";
            var invalid = System.IO.Path.GetInvalidFileNameChars();
            var sb = new System.Text.StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                bool bad = ch == '\\' || ch == '/' || ch == ':' || ch == '*' || ch == '?' || ch == '"' || ch == '<' || ch == '>' || ch == '|';
                if (!bad)
                    for (int j = 0; j < invalid.Length; j++) if (ch == invalid[j]) { bad = true; break; }
                sb.Append(bad ? '_' : ch);
            }
            return sb.ToString();
        }

    }
}
