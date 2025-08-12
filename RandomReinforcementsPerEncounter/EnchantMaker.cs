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
        // Llama a esto en el runner
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
                energyWord: "acid damage"
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
                energyWord: "fire damage"
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
                energyWord: "cold damage"
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
                energyWord: "electricity damage"
            );

            // Thundering – base vanilla: 1d6 sonic
            RegisterTiersFor(
                baseGuid: "7bda5277d36ad114f9f9fd21d0dab658",
                seedRoot1: "thundering.t1",////////////////////////////
                seedRoot2: "thundering.t2",//                        //
                seedRoot3: "thundering.t3",//       Dont Touch       //
                seedRoot4: "thundering.t4",//     seed for GUID      //
                seedRoot5: "thundering.t5",//                        //
                seedRoot6: "thundering.t6",////////////////////////////
                nameRoot: "Thundering",
                energyWord: "sonic damage"
            );

            // Greater Necrotic – base vanilla: 2d6 negative energy damage
            RegisterTiersFor(
                baseGuid: "c2229230ff9292048b07a8429d6536c6",
                seedRoot1: "necrotic.t1",////////////////////////////
                seedRoot2: "necrotic.t2",//                        //
                seedRoot3: "necrotic.t3",//       Dont Touch       //
                seedRoot4: "necrotic.t4",//     seed for GUID      //
                seedRoot5: "necrotic.t5",//                        //
                seedRoot6: "necrotic.t6",////////////////////////////
                nameRoot: "Necrotic",
                energyWord: "negative energy damage"
            );

            // LightHammerOfRighteousnessEnchantment – base vanilla: 1 holy
            RegisterTiersFor(
                baseGuid: "dd78f3841a990c44cb8e8f9c4f615879",
                seedRoot1: "holy.t1",////////////////////////////
                seedRoot2: "holy.t2",//                        //
                seedRoot3: "holy.t3",//       Dont Touch       //
                seedRoot4: "holy.t4",//     seed for GUID      //
                seedRoot5: "holy.t5",//                        //
                seedRoot6: "holy.t6",////////////////////////////
                nameRoot: "Holy",
                energyWord: "holy damage"
            );

        }

        private static void RegisterTiersFor(
            string baseGuid,
            string seedRoot1,
            string seedRoot2,
            string seedRoot3,
            string seedRoot4,
            string seedRoot5,
            string seedRoot6,
            string nameRoot,
            string energyWord
        )
        {
            var baseBp = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(baseGuid);
            if (baseBp == null) { Debug.LogError("[RRE] Base not found: " + nameRoot); return; }

            var tiers = new (string name, string seed, DiceFormula dice, string diceText)[]
            {
                ($"{nameRoot} 1", $"{seedRoot1}", new DiceFormula(1, DiceType.D3),  "1d3"),
                ($"{nameRoot} 2", $"{seedRoot2}", new DiceFormula(1, DiceType.D6),  "1d6"),
                ($"{nameRoot} 3", $"{seedRoot3}", new DiceFormula(1, DiceType.D10), "1d10"),
                ($"{nameRoot} 4", $"{seedRoot4}", new DiceFormula(2, DiceType.D6),  "2d6"),
                ($"{nameRoot} 5", $"{seedRoot5}", new DiceFormula(2, DiceType.D8),  "2d8"),
                ($"{nameRoot} 6", $"{seedRoot6}", new DiceFormula(2, DiceType.D10), "2d10"),
            };

            for (int i = 0; i < tiers.Length; i++)
                MakeAndRegisterTier(baseBp, tiers[i].name, tiers[i].seed, tiers[i].dice, tiers[i].diceText, nameRoot, energyWord);
        }

        private static void MakeAndRegisterTier(
            BlueprintWeaponEnchantment baseBp,
            string name,
            string guidSeed,
            DiceFormula dice,
            string diceText,
            string prefix,
            string energyWord
        )
        {
            var gid = GuidUtil.FromString(guidSeed);
            var gidStr = gid.ToString();

            // Evita duplicados (idempotente)
            var already = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(gidStr);
            if (already != null) { Debug.Log("[RRE] already present: " + name); return; }

            // Shallow clone del blueprint base (conserva FX/prefab/elemento)
            var mwc = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            var bp = (BlueprintWeaponEnchantment)mwc.Invoke(baseBp, null);
            bp.name = "MOD_" + name.Replace(' ', '_'); // nombre Unity único

            // (opcional) alinear AssetGuid interno
            TrySetGuidDeep(bp, gid);

            // Clonar componentes y ajustar WeaponEnergyDamageDice (solo los dados)
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
                        // No tocamos el tipo de energía: lo hereda del base (ácido/fuego)
                        we.EnergyDamageDice = dice;
                    }

                    // OwnerBlueprint si existe
                    var fiOwner = cClone.GetType().GetField("OwnerBlueprint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (fiOwner != null) { try { fiOwner.SetValue(cClone, bp); } catch { } }

                    compsNew[i] = cClone;
                }
                bp.ComponentsArray = compsNew;
            }

            // Textos
            SetStr(bp, "Name", name);
            SetStr(bp, "Description",
                $"This weapon deals an extra <b><color=#703565><link=\"Encyclopedia:Dice\">{diceText}</link></color></b> points of <b><color=#703565><link=\"Encyclopedia:Energy_Damage\">{energyWord}</link></color></b> on a successful hit.");
            SetStr(bp, "Prefix", prefix);
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
