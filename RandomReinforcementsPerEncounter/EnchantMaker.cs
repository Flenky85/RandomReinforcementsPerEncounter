using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static Kingmaker.RuleSystem.RulebookEvent;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Newtonsoft.Json;



namespace RandomReinforcementsPerEncounter
{
    public static class EnchantMaker
    {
        public static void RegisterElementalTiers()
        {
            // Corrosive – base vanilla: 1d6 acid
            RegisterElementalTiersFor(
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
            RegisterElementalTiersFor(
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
            RegisterElementalTiersFor(
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
            RegisterElementalTiersFor(
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
            RegisterElementalTiersFor(
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
            RegisterElementalTiersFor(
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
            RegisterElementalTiersFor(
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
        private static void RegisterElementalTiersFor(
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
                (nameRoot + " 1", seedRoot1, new DiceFormula(1, DiceType.D3),  "1d3"),
                (nameRoot + " 2", seedRoot2, new DiceFormula(1, DiceType.D6),  "1d6"),
                (nameRoot + " 3", seedRoot3, new DiceFormula(1, DiceType.D10), "1d10"),
                (nameRoot + " 4", seedRoot4, new DiceFormula(2, DiceType.D6),  "2d6"),
                (nameRoot + " 5", seedRoot5, new DiceFormula(2, DiceType.D8),  "2d8"),
                (nameRoot + " 6", seedRoot6, new DiceFormula(2, DiceType.D10), "2d10"),
            };

            for (int i = 0; i < tiers.Length; i++)
            {
                var t = tiers[i];
                try
                {
                    MakeAndRegisterElementalTier(baseBp, t.name, t.seed, t.dice, t.diceText, nameRoot, energyDamage);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("[RRE] Failed to register tier '" + t.name + "' seed='" + t.seed + "': " + ex);
                }
            }
        }

        private static void MakeAndRegisterElementalTier(
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

            if (EnchantMakerUtils.BlueprintExists(gidStr))
            {
                Debug.Log("[RRE] already present: " + name);
                return;
            }

            
            var bp = EnchantMakerUtils.MakeCloneWithGuid(baseBp, name, gid);

            
            EnchantMakerUtils.CopyComponentsWithOwnerRemap(
                baseBp, bp,
                c =>
                {
                    if (c is WeaponEnergyDamageDice we)
                        we.EnergyDamageDice = dice;
                    return c;
                }
            );

            
            var diceTextUniform = EnchantMakerUtils.DiceToText(dice);
            var newDesc = "This weapon deals an extra <b><color=#703565><link=\"Encyclopedia:Dice\">" + diceTextUniform
             + "</link></color></b> points of <b><color=#703565><link=\"Encyclopedia:Energy_Damage\">"
             + energyDamage + "</link></color></b> on a successful hit.";

            EnchantMakerUtils.SetEnchantTextViaPack(bp, gidStr, name, prefix, "", newDesc);
            EnchantMakerUtils.EnableAndRegister(bp, gid);
        }

        // ===== Tiers de salvación (Nightmare como ejemplo) =====

        public static void RegisterDebuffTiers()
        {
            RegisterDebuffTiersFor(
                baseGuid: "95cebcc4740916140be22de6e2c5d060", // ScreamOfPainEnchantment base
                seedRoot1: "shaken.t1",////////////////////////////
                seedRoot2: "shaken.t2",//                        //
                seedRoot3: "shaken.t3",//       Dont Touch       //
                seedRoot4: "shaken.t4",//     seed for GUID      //
                seedRoot5: "shaken.t5",//                        //
                seedRoot6: "shaken.t6",////////////////////////////
                nameRoot: "Shaken"
            );
        }

        private static void RegisterDebuffTiersFor(
            string baseGuid,
            string seedRoot1, string seedRoot2, string seedRoot3,
            string seedRoot4, string seedRoot5, string seedRoot6,
            string nameRoot)
        {
            var baseBp = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(baseGuid);
            if (baseBp == null) { Debug.LogError("[RRE] Base not found: " + nameRoot); return; }

            var tiers = new (string name, string seed, int dc)[]
            {
                (nameRoot + " 1", seedRoot1, 13),
                (nameRoot + " 2", seedRoot2, 17),
                (nameRoot + " 3", seedRoot3, 21),
                (nameRoot + " 4", seedRoot4, 25),
                (nameRoot + " 5", seedRoot5, 29),
                (nameRoot + " 6", seedRoot6, 33),
            };

            for (int i = 0; i < tiers.Length; i++)
            {
                var t = tiers[i];
                try
                {
                    MakeAndRegisterDebuffTier(baseBp, t.name, t.seed, t.dc, nameRoot);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("[RRE] Failed to register tier '" + t.name + "' seed='" + t.seed + "': " + ex);
                }
            }
        }

        private static void MakeAndRegisterDebuffTier(
            BlueprintWeaponEnchantment baseBp,
            string name,
            string guidSeed,
            int dc,
            string prefix)
        {
            var gid = GuidUtil.FromString(guidSeed);
            var gidStr = gid.ToString();

            if (EnchantMakerUtils.BlueprintExists(gidStr))
            {
                Debug.Log("[RRE] already present: " + name);
                return;
            }

            // 1) Clon raíz del enchant (ScriptableObject) con GUID nuevo
            var bp = EnchantMakerUtils.MakeCloneWithGuid(baseBp, name, gid);

            // 2) Copiar componentes remapeando Owner y fijar DC en ContextSetAbilityParams
            EnchantMakerUtils.CopyComponentsWithOwnerRemap(
                baseBp, bp,
                c =>
                {
                    if (c is ContextSetAbilityParams sp)
                    {
                        var v = sp.DC;
                        v.ValueType = ContextValueType.Simple;
                        v.Value = dc;
                        sp.DC = v;
                    }
                    // <<< limpiar el trigger clonado: sin acciones >>>
                    if (c is AddInitiatorAttackWithWeaponTrigger t)
                    {
                        t.Action = new ActionList { Actions = System.Array.Empty<GameAction>() };
                    }
                    return c;
                });

            // === 3) CLONAR EN PROFUNDIDAD LOS 3 ELEMENTS DEL ACTION LIST ===
            // Trigger del CLON (donde vamos a inyectar)
            var triggerClone = bp.ComponentsArray
                .OfType<AddInitiatorAttackWithWeaponTrigger>().FirstOrDefault();

            // Trigger del BASE (de aquí sacamos la cadena original)
            var triggerBase = baseBp.ComponentsArray
                .OfType<AddInitiatorAttackWithWeaponTrigger>().FirstOrDefault();

            if (triggerClone == null || triggerBase?.Action?.Actions == null ||
                triggerBase.Action.Actions.Length == 0 ||
                !(triggerBase.Action.Actions[0] is ContextActionSavingThrow stOld))
            {
                Debug.LogError("[RRE] Layout inesperado: no hay SavingThrow en raíz del base.");
                return;
            }

            // Clonar SavingThrow (con hijos)
            var stSeed = $"{guidSeed}:/enchant/actions/0/saving";
            var stNewGuid = GuidUtil.SaveGuid(stSeed);
            var stNew = (ContextActionSavingThrow)DeepCloneWithGuid(stOld, stNewGuid, bp);

            // Forzar a usar DC de contexto
            stNew.UseDCFromContextSavingThrow = true;
            stNew.HasCustomDC = false;

            // Tomar ConditionalSaved y ApplyBuff **del CLON**
            var condNew = stNew.Actions?.Actions?.Length > 0 ? stNew.Actions.Actions[0] as ContextActionConditionalSaved : null;
            var apNew = condNew?.Failed?.Actions?.Length > 0 ? condNew.Failed.Actions[0] as ContextActionApplyBuff : null;
            if (condNew == null || apNew == null)
            {
                Debug.LogError("[RRE] No se encontró ConditionalSaved/ApplyBuff dentro del SavingThrow clonado.");
                return;
            }

            // Mantener el mismo BlueprintBuff del BASE (no clonar el buff)
            var condOld = stOld.Actions?.Actions?.Length > 0 ? stOld.Actions.Actions[0] as ContextActionConditionalSaved : null;
            var apOld = condOld?.Failed?.Actions?.Length > 0 ? condOld.Failed.Actions[0] as ContextActionApplyBuff : null;
            if (apOld?.m_Buff != null)
                apNew.m_Buff = apOld.m_Buff;

            // Reinyectar: sustituye por completo el ActionList del trigger CLONADO
            triggerClone.Action = new ActionList
            {
                Actions = new GameAction[] { (GameAction)stNew }
            };

            var stNew2 = (ContextActionSavingThrow)triggerClone.Action.Actions[0];
            var condNew2 = (ContextActionConditionalSaved)stNew2.Actions.Actions[0];
            var apNew2 = (ContextActionApplyBuff)condNew2.Failed.Actions[0];

            string G(object o, string n)
            {
                var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                var p = o.GetType().GetProperty(n, flags);
                if (p != null) return p.GetValue(o, null)?.ToString();
                var f = o.GetType().GetField(n, flags);
                if (f != null) return f.GetValue(o)?.ToString();
                return "<?>";
            }

            Debug.Log($"[RRE] ST guid={G(stNew, "AssetGuid")} owner={G(stNew, "Owner")}");
            Debug.Log($"[RRE] CS guid={G(condNew, "AssetGuid")} owner={G(condNew, "Owner")}");
            Debug.Log($"[RRE] AP guid={G(apNew, "AssetGuid")} owner={G(apNew, "Owner")}");



            Debug.Log("[RRE] Debuff chain cloned (SavingThrow -> ConditionalSaved -> ApplyBuff).");

            var spNew = bp.ComponentsArray.OfType<ContextSetAbilityParams>().FirstOrDefault();
            Debug.Log($"[RRE] {name}: ContextSetAbilityParams.DC = {spNew?.DC.Value}");




            // 4) Texto
            var conditionProper = prefix;
            var conditionLower = prefix.ToLowerInvariant();
            var newDesc =
                "Each time this weapon lands a hit, the enemy has to pass Saving Throw or be " +
                $"<b><color=#703565><link=\"Condition{conditionProper}\">{conditionLower}</link></color></b> for 1d3 " +
                "<b><color=#703565><link=\"Encyclopedia:Combat_Round\">rounds</link></color></b>. " +
                $"(<b><color=#703565><link=\"Encyclopedia:DC\">DC</link></color></b> {dc})";

            EnchantMakerUtils.SetEnchantTextViaPack(bp, gidStr, name, prefix, "", newDesc);

            // 5) Registrar el enchant clonado
            EnchantMakerUtils.EnableAndRegister(bp, gid);
            
            Debug.Log($"[RRE] Registered debuff enchant '{name}' {gidStr}");
            var enchCheck = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(gidStr);
            Debug.Log($"[RRE] Enchant cache: {(enchCheck != null ? "FOUND" : "MISSING")} {gidStr}");

            // ---- helpers locales ----
            object DeepCloneWithGuid(object actionObj, string dashedGuid, BlueprintWeaponEnchantment newOwner)
            {
                if (actionObj == null) return null;

                // 1) Shallow clone del GameAction (sin JSON)
                var cloned = EnchantMakerUtils.ShallowClone(actionObj);

                // 2) Intenta fijar AssetGuid / Owner si existen
                SetMemberIfExists(cloned, "AssetGuid", dashedGuid);
                SetMemberIfExists(cloned, "AssetGuidShort", dashedGuid.Length >= 4 ? dashedGuid.Substring(0, 4) : dashedGuid);
                SetMemberIfExists(cloned, "Owner", newOwner);

                // 3) Clonado recursivo de subacciones / ActionList / arrays de GameAction
                CloneActionChildren(cloned, newOwner, dashedGuid);

                return cloned;
            }

            void CloneActionChildren(object obj, BlueprintWeaponEnchantment newOwner, string seedRoot)
            {
                if (obj == null) return;
                var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                foreach (var f in obj.GetType().GetFields(flags))
                {
                    var ft = f.FieldType;

                    // --- ActionList única ---
                    if (ft == typeof(ActionList))
                    {
                        var al = (ActionList)f.GetValue(obj);
                        if (al?.Actions == null) continue;

                        var clonedActions = new List<GameAction>(al.Actions.Length);
                        for (int i = 0; i < al.Actions.Length; i++)
                        {
                            var child = al.Actions[i];
                            if (child == null) { clonedActions.Add(null); continue; }

                            var childGuid = GuidUtil.CondGuid($"{seedRoot}:/nested/{f.Name}/{i}");
                            var childClone = (GameAction)DeepCloneWithGuid(child, childGuid, newOwner);
                            clonedActions.Add(childClone);
                        }

                        f.SetValue(obj, new ActionList { Actions = clonedActions.ToArray() });
                    }
                    // --- Arrays de GameAction ---
                    else if (ft.IsArray && typeof(GameAction).IsAssignableFrom(ft.GetElementType()))
                    {
                        var arr = (GameAction[])f.GetValue(obj);
                        if (arr == null) continue;

                        var cloned = new GameAction[arr.Length];
                        for (int i = 0; i < arr.Length; i++)
                        {
                            var child = arr[i];
                            if (child == null) { cloned[i] = null; continue; }

                            var childGuid = GuidUtil.CondGuid($"{seedRoot}:/nested/{f.Name}/{i}");
                            cloned[i] = (GameAction)DeepCloneWithGuid(child, childGuid, newOwner);
                        }
                        f.SetValue(obj, cloned);
                    }
                    // --- Sub‑acción única incrustada ---
                    else if (typeof(GameAction).IsAssignableFrom(ft))
                    {
                        var sub = f.GetValue(obj);
                        if (sub != null)
                        {
                            var childGuid = GuidUtil.CondGuid($"{seedRoot}:/nested/{f.Name}");
                            var subClone = DeepCloneWithGuid(sub, childGuid, newOwner);
                            f.SetValue(obj, subClone);
                        }
                    }
                    // IMPORTANTE: No clonamos ScriptableObjects / Blueprints / Properties
                    // (BlueprintUnitProperty, ScriptableObject, etc.) -> se dejan por referencia.
                }
            }

            /*
            void SetFieldIfExists(object obj, string fieldName, object value)
            {
                var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                var fi = obj.GetType().GetField(fieldName, flags);
                if (fi != null) fi.SetValue(obj, value);
            }*/
            
            void SetMemberIfExists(object obj, string memberName, object value)
            {
                var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                // Preferir propiedad si existe
                var pi = obj.GetType().GetProperty(memberName, flags);
                if (pi != null && pi.CanWrite)
                {
                    try { pi.SetValue(obj, value, null); return; } catch { /* ignore */ }
                }

                // Si no, intentar campo
                var fi = obj.GetType().GetField(memberName, flags);
                if (fi != null)
                {
                    try { fi.SetValue(obj, value); return; } catch { /* ignore */ }
                }
            }
        }

    }
}
