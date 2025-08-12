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
    }
}
