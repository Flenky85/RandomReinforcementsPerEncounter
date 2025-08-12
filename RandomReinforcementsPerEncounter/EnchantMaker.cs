using System.Reflection;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    public static class EnchantMaker
    {
        // Llama a esto en el runner/init
        public static void RegisterElementalTiers()
        {
            // Corrosive – base vanilla: 1d6 acid
            RegisterTiersFor(
                baseGuid: "633b38ff1d11de64a91d490c683ab1c8",
                seedRoot1: "corrosive.t1",////////////////////////////
                seedRoot2: "corrosive.t2",//                        //
                seedRoot3: "corrosive.t3",//       Dont Touch       //
                seedRoot4: "corrosive.t4",//     seed for GUID      //
                seedRoot5: "corrosive.t5",//                        //
                seedRoot6: "corrosive.t6",////////////////////////////
                nameRoot: "Corrosive",
                energyDamage: "acid damage"
            );

            // Flaming – base vanilla: 1d6 fire
            RegisterTiersFor(
                baseGuid: "30f90becaaac51f41bf56641966c4121",
                seedRoot1: "flaming.t1",////////////////////////////
                seedRoot2: "flaming.t2",//                        //
                seedRoot3: "flaming.t3",//       Dont Touch       //
                seedRoot4: "flaming.t4",//     seed for GUID      //
                seedRoot5: "flaming.t5",//                        //
                seedRoot6: "flaming.t6",////////////////////////////
                nameRoot: "Flaming",
                energyDamage: "fire damage"
            );

            // Frost – base vanilla: 1d6 cold
            RegisterTiersFor(
                baseGuid: "421e54078b7719d40915ce0672511d0b",
                seedRoot1: "frost.t1",////////////////////////////
                seedRoot2: "frost.t2",//                        //
                seedRoot3: "frost.t3",//       Dont Touch       //
                seedRoot4: "frost.t4",//     seed for GUID      //
                seedRoot5: "frost.t5",//                        //
                seedRoot6: "frost.t6",////////////////////////////
                nameRoot: "Frost",
                energyDamage: "cold damage"
            );

            // Shock – base vanilla: 1d6 electricity
            RegisterTiersFor(
                baseGuid: "7bda5277d36ad114f9f9fd21d0dab658",
                seedRoot1: "shock.t1",////////////////////////////
                seedRoot2: "shock.t2",//                        //
                seedRoot3: "shock.t3",//       Dont Touch       //
                seedRoot4: "shock.t4",//     seed for GUID      //
                seedRoot5: "shock.t5",//                        //
                seedRoot6: "shock.t6",////////////////////////////
                nameRoot: "Shock",
                energyDamage: "electricity damage"
            );

            // Thundering – base vanilla: 1d6 sonic
            RegisterTiersFor(
                baseGuid: "690e762f7704e1f4aa1ac69ef0ce6a96",
                seedRoot1: "thundering.t1",////////////////////////////
                seedRoot2: "thundering.t2",//                        //
                seedRoot3: "thundering.t3",//       Dont Touch       //
                seedRoot4: "thundering.t4",//     seed for GUID      //
                seedRoot5: "thundering.t5",//                        //
                seedRoot6: "thundering.t6",////////////////////////////
                nameRoot: "Thundering",
                energyDamage: "sonic damage"
            );

            // Necrotic – base vanilla: 2d6 negative (greater)
            RegisterTiersFor(
                baseGuid: "c2229230ff9292048b07a8429d6536c6",
                seedRoot1: "necrotic.t1",////////////////////////////
                seedRoot2: "necrotic.t2",//                        //
                seedRoot3: "necrotic.t3",//       Dont Touch       //
                seedRoot4: "necrotic.t4",//     seed for GUID      //
                seedRoot5: "necrotic.t5",//                        //
                seedRoot6: "necrotic.t6",////////////////////////////
                nameRoot: "Necrotic",
                energyDamage: "negative energy damage"
            );

            // Holy (LightHammerOfRighteousnessEnchantment) – base vanilla: 1 holy
            RegisterTiersFor(
                baseGuid: "dd78f3841a990c44cb8e8f9c4f615879",
                seedRoot1: "holy.t1",////////////////////////////
                seedRoot2: "holy.t2",//                        //
                seedRoot3: "holy.t3",//       Dont Touch       //
                seedRoot4: "holy.t4",//     seed for GUID      //
                seedRoot5: "holy.t5",//                        //
                seedRoot6: "holy.t6",////////////////////////////
                nameRoot: "Holy",
                energyDamage: "holy damage"
            );

        }

        private static void RegisterTiersFor(
            string baseGuid,
            string seedRoot1, string seedRoot2, string seedRoot3,
            string seedRoot4, string seedRoot5, string seedRoot6,
            string nameRoot,
            string energyDamage)
        {
            var baseBp = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(baseGuid);
            if (baseBp == null) { Debug.LogError("[RRE] Base not found: " + nameRoot); return; }

            var tiers = new (string name, string seed, DiceFormula dice, string diceText)[]
            {
                ($"{nameRoot} 1", seedRoot1, new DiceFormula(1, DiceType.D3),  "1d3"),
                ($"{nameRoot} 2", seedRoot2, new DiceFormula(1, DiceType.D6),  "1d6"),
                ($"{nameRoot} 3", seedRoot3, new DiceFormula(1, DiceType.D10), "1d10"),
                ($"{nameRoot} 4", seedRoot4, new DiceFormula(2, DiceType.D6),  "2d6"),
                ($"{nameRoot} 5", seedRoot5, new DiceFormula(2, DiceType.D8),  "2d8"),
                ($"{nameRoot} 6", seedRoot6, new DiceFormula(2, DiceType.D10), "2d10"),
            };

            for (int i = 0; i < tiers.Length; i++)
            {
                var (tName, tSeed, tDice, tText) = tiers[i];
                try { MakeAndRegisterTier(baseBp, tName, tSeed, tDice, tText, nameRoot, energyDamage); }
                catch (System.Exception ex) { Debug.LogError($"[RRE] Failed to register tier '{tName}' seed='{tSeed}': {ex}"); }
            }
        }

        private static void MakeAndRegisterTier(
            BlueprintWeaponEnchantment baseBp,
            string name,
            string guidSeed,
            DiceFormula dice,
            string diceText,
            string prefix,
            string energyDamage)
        {
            var gid = GuidUtil.FromString(guidSeed);
            var gidStr = gid.ToString();

            if (ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(gidStr) != null)
            {
                Debug.Log("[RRE] already present: " + name);
                return;
            }

            // clone (shallow)
            var mwc = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            var bp = (BlueprintWeaponEnchantment)mwc.Invoke(baseBp, null);
            bp.name = "RRE_" + name.Replace(' ', '_');

            TrySetGuidDeep(bp, gid);

            // copy components & tweak dice (mantiene el tipo de energía del base)
            var compsBase = baseBp.ComponentsArray;
            if (compsBase != null)
            {
                var compsNew = new BlueprintComponent[compsBase.Length];
                for (int i = 0; i < compsBase.Length; i++)
                {
                    var c = compsBase[i];
                    var cClone = c == null ? null : (BlueprintComponent)mwc.Invoke(c, null);
                    if (cClone is WeaponEnergyDamageDice we) we.EnergyDamageDice = dice;
                    var fiOwner = cClone?.GetType().GetField("OwnerBlueprint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (fiOwner != null) { try { fiOwner.SetValue(cClone, bp); } catch { } }
                    compsNew[i] = cClone;
                }
                bp.ComponentsArray = compsNew;
            }

            // descripción uniforme con tu plantilla
            var diceTextUniform = DiceToText(dice);
            var newDesc = BuildUniformDescription(diceTextUniform, energyDamage);

            // LocalizedStrings: claves únicas por GUID y SIEMPRE instancia nueva (no compartida)
            SetEnchantTextViaPack(bp, gidStr, name, prefix, "", newDesc);

            // refresh & register
            try
            {
                var onEnable = typeof(BlueprintScriptableObject).GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                onEnable?.Invoke(bp, null);
            }
            catch { }

            ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(gid, bp);
            var test = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(gidStr);
        }

        // ===== helpers =====

        private static string BuildUniformDescription(string diceText, string energyWord)
        {
            // Tu formato exacto
            return $"This weapon deals an extra <b><color=#703565><link=\"Encyclopedia:Dice\">{diceText}</link></color></b> points of <b><color=#703565><link=\"Encyclopedia:Energy_Damage\">{energyWord}</link></color></b> on a successful hit.";
        }

        private static string DiceToText(DiceFormula d)
        {
            var s = d.ToString();
            if (!string.IsNullOrEmpty(s)) return s;

            // Fallback defensivo por si cambia la impl
            try
            {
                var t = d.GetType();
                var rolls = (int)(t.GetField("m_Rolls", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.GetValue(d) ?? 1);
                var dice = t.GetField("m_Dice", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.GetValue(d);
                var dieS = dice?.ToString()?.TrimStart('D') ?? "6";
                return $"{rolls}d{dieS}";
            }
            catch { return "1d6"; }
        }

        // Escribe en el pack y fija Key/Shared/ShouldProcess en LocalizedString (nuevas instancias)
        private static void SetEnchantTextViaPack(BlueprintWeaponEnchantment bp, string gidStr, string name, string prefix, string suffix, string desc)
        {
            if (LocalizationManager.CurrentPack == null)
                LocalizationManager.WaitForInit();

            PutAndPointLS(bp, "m_EnchantName", $"rre.{gidStr}.name", name);
            PutAndPointLS(bp, "m_Prefix", $"rre.{gidStr}.pref", prefix ?? "");
            PutAndPointLS(bp, "m_Suffix", $"rre.{gidStr}.suff", suffix ?? "");
            PutAndPointLS(bp, "m_Description", $"rre.{gidStr}.desc", desc ?? "");
        }

        private static void PutAndPointLS(object owner, string fieldName, string key, string text)
        {
            LocalizationManager.CurrentPack?.PutString(key, text ?? "");

            var field = GetFieldDeep(owner.GetType(), fieldName);
            if (field == null) return;

            var lsType = field.FieldType; // Kingmaker.Localization.LocalizedString
            var newLs = System.Activator.CreateInstance(lsType);

            var keyProp = lsType.GetProperty("Key", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var procProp = lsType.GetProperty("ShouldProcess", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var sharedFi = lsType.GetField("Shared", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            try { keyProp?.SetValue(newLs, key, null); } catch { }
            try { procProp?.SetValue(newLs, true, null); } catch { }
            try { sharedFi?.SetValue(newLs, null); } catch { }

            field.SetValue(owner, newLs);
        }

        private static FieldInfo GetFieldDeep(System.Type t, string name)
        {
            while (t != null)
            {
                var fi = t.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (fi != null) return fi;
                t = t.BaseType;
            }
            return null;
        }

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
    }
}
