using System.Reflection;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    public static class EnchantMaker
    {
        // Llama a esto en el runner (abajo)
        public static void RegisterCorrosiveAcidTiers()
        {
            const string BaseGuid = "633b38ff1d11de64a91d490c683ab1c8"; // corrosive 1d6 vanilla
            var baseBp = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(BaseGuid);
            if (baseBp == null) { Debug.LogError("[RRE] Corrosive base not found."); return; }

            var tiers = new (string name, string seed, DiceFormula dice, string diceText)[]
            {
                ("Corrosive 1", "corrosive.t1", new DiceFormula(1, DiceType.D3),  "1d3"),
                ("Corrosive 2", "corrosive.t2", new DiceFormula(1, DiceType.D6),  "1d6"),
                ("Corrosive 3", "corrosive.t3", new DiceFormula(1, DiceType.D10), "1d10"),
                ("Corrosive 4", "corrosive.t4", new DiceFormula(2, DiceType.D6),  "2d6"),
                ("Corrosive 5", "corrosive.t5", new DiceFormula(2, DiceType.D8),  "2d8"),
                ("Corrosive 6", "corrosive.t6", new DiceFormula(2, DiceType.D10), "2d10"),
            };

            for (int i = 0; i < tiers.Length; i++)
                MakeAndRegisterCorrosiveTier(baseBp, tiers[i].name, tiers[i].seed, tiers[i].dice, tiers[i].diceText);

            Debug.Log("[RRE] Corrosive acid tiers registered.");
        }

        private static void MakeAndRegisterCorrosiveTier(
            BlueprintWeaponEnchantment baseBp,
            string name,
            string guidSeed,
            DiceFormula dice,
            string diceText)
        {
            var gid = GuidUtil.FromString(guidSeed);
            var gidStr = gid.ToString();

            // Evita duplicados (idempotente)
            var already = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(gidStr);
            if (already != null) { Debug.Log("[RRE] already present: " + name); return; }

            // Shallow clone del blueprint base (conserva FX/prefab)
            var mwc = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            var bp = (BlueprintWeaponEnchantment)mwc.Invoke(baseBp, null);
            bp.name = "MOD_" + name.Replace(' ', '_'); // nombre Unity único

            // (opcional) alinear AssetGuid interno (no imprescindible para la caché)
            TrySetGuidDeep(bp, gid);

            // Clonar componentes y ajustar WeaponEnergyDamageDice
            var compsBase = baseBp.ComponentsArray;
            if (compsBase != null)
            {
                var compsNew = new BlueprintComponent[compsBase.Length];
                for (int i = 0; i < compsBase.Length; i++)
                {
                    var c = compsBase[i];
                    if (c == null) { compsNew[i] = null; continue; }

                    var cClone = (BlueprintComponent)mwc.Invoke(c, null);

                    var we = cClone as WeaponEnergyDamageDice;
                    if (we != null)
                    {
                        we.EnergyDamageDice = dice;
                    }

                    // OwnerBlueprint si existe (según build)
                    var fiOwner = cClone.GetType().GetField("OwnerBlueprint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (fiOwner != null) { try { fiOwner.SetValue(cClone, bp); } catch { } }

                    compsNew[i] = cClone;
                }
                bp.ComponentsArray = compsNew;
            }

            // Textos (strings en tu build)
            SetStr(bp, "Name", name);
            SetStr(bp, "Description",
                $"Corrosive weapon deals an extra <b><color=#703565><link=\"Encyclopedia:Dice\">{diceText}</link></color></b> points of <b><color=#703565><link=\"Encyclopedia:Energy_Damage\">acid damage</link></color></b> on a successful hit.");
            SetStr(bp, "Prefix", "Corrosive");
            SetStr(bp, "Suffix", "");

            // Registro correcto en la caché global
            ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(gid, bp);
            Debug.Log("[RRE] registered: " + name + " → " + gidStr);
        }

        // ==== helpers ====
        private static bool TrySetGuidDeep(object obj, BlueprintGuid value)
        {
            var t = obj.GetType();
            while (t != null)
            {
                var pi = t.GetProperty("AssetGuid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (pi != null && pi.CanWrite) { try { pi.SetValue(obj, value, null); return true; } catch { } }
                var fi = t.GetField("AssetGuid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fi != null) { try { fi.SetValue(obj, value); return true; } catch { } }
                var fi2 = t.GetField("m_AssetGuid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fi2 != null) { try { fi2.SetValue(obj, value); return true; } catch { } }
                t = t.BaseType;
            }
            return false;
        }

        private static void SetStr(object obj, string member, string val)
        {
            if (val == null) val = "";
            var t = obj.GetType();
            var pi = t.GetProperty(member, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (pi != null && pi.CanWrite && pi.PropertyType == typeof(string)) { try { pi.SetValue(obj, val, null); return; } catch { } }
            var fi = t.GetField(member, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fi != null && fi.FieldType == typeof(string)) { try { fi.SetValue(obj, val); return; } catch { } }
            var fi2 = t.GetField("m_" + member, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fi2 != null && fi2.FieldType == typeof(string)) { try { fi2.SetValue(obj, val); return; } catch { } }
        }
    }
}
