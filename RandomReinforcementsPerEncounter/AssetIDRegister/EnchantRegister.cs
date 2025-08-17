using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static RandomReinforcementsPerEncounter.EnchantFactory;

namespace RandomReinforcementsPerEncounter
{
    internal class EnchantRegister
    {

        // Prefabs
        private const string PREFAB_CRIMSON_MOON = "907d3c215c9522d4e8a3b763f1b32935"; // CrimsonMoonEnchantment (blood)
        private const string PREFAB_UNHOLY = "e098bc50ec458af4cb7c2d828db0ce18"; // Unholy
        private const string PREFAB_CORROSIVE = "bfafef74d59950242915a8e294e6fac0"; // Corrosive
        private const string PREFAB_FLAMING = "91e5a56dd421a2941984a36a2af164b6"; // Flaming
        private const string PREFAB_FROST = "e9930f40a35b67c418e78a98c601c93b"; // Frost
        private const string PREFAB_SHOCK = "1d1465ffa2699644ba8dfac48cb33195"; // Shock
        private const string PREFAB_VICIOUS = "d01d77862c68852449895718902c8599"; // Vicious (Thorns)
        private const string PREFAB_HOLY = "d739a9e236ba6164ab854b356bfb6ed5"; // Holy
        private const string PREFAB_SONIC = "d31b9df8d99674742a161eb3faa07f3f"; // Sonic
        private const string PREFAB_SPEED = "eee18332ea407bb4ea9bf2aa5f9ddf90"; // Speed (wind green)
        private const string PREFAB_BRILLIANT_ENERGY = "fdc7f8f37d3f8da42be2a1d35a617001"; // BrilliantEnergy (magic blue)
        private const string PREFAB_AXIOMATIC = "9c950f7e0624df24ca74d9b01a3a2cfa"; // Axiomatic (yellow circles)
        private const string PREFAB_AGILE = "a4eba3360cc5b5d4ba2fe1036ce0cc8c"; // Agile (wind blue)
        private const string PREFAB_GHOST = "d7b9bfb16264e4d4aad2abef2c80f835"; // Ghost
        private const string PREFAB_ANARCHIC = "57315bc1e1f62a741be0efde688087e9"; // Anarchic (oxido)

        public static void RegisterAll()
        {
            //////////////////////////////////////////////////////////////////////////////////
            //                            Debuff onlyHit                                    //
            //////////////////////////////////////////////////////////////////////////////////
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t6").ToString(), DC = 26 }
                },
                name: "Fearsome",
                nameRoot: "shaken",
                description: "shaken",
                buff: "25ec6cb6ab1845c48a95f9c20b034220",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyHit
            ); 
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t6").ToString(), DC = 26 }
                },
                name: "Blinding",
                nameRoot: "blindness",
                description:"blinded",
                buff: "0ec36e7596a4928489d2049e1e1c76a7",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t6").ToString(), DC = 26 }
                },
                name: "Dazzling",
                nameRoot: "dazzled",
                description: "dazzled",
                buff: "df6d1020da07524423afbae248845ecc",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t6").ToString(), DC = 26 }
                },
                name: "Sickening",
                nameRoot: "sickened",
                description: "sickened",
                buff: "4e42460798665fd4cb9143ffa7ada323",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t6").ToString(), DC = 26 }
                },
                name: "Staggering",
                nameRoot: "staggered",
                description: "staggered",
                buff: "df3950af5a783bd4d91ab73eb8fa0fd3",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t6").ToString(), DC = 26 }
                },
                name: "Fatiguing",
                nameRoot: "fatigue",
                description: "fatigued",
                buff: "e6f2fc5d73d88064583cb828801172f4",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t6").ToString(), DC = 26 }
                },
                name: "Confusing",
                nameRoot: "confusion",
                description: "confused",
                buff: "886c7407dc623dc499b9f1465ff382df",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t6").ToString(), DC = 26 }
                },
                name: "Entangling",
                nameRoot: "entangled",
                description: "entangled",
                buff: "f7f6260726117cf4b90a6086b05d2e38",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Reflex,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t6").ToString(), DC = 26 }
                },
                name: "Slowing",
                nameRoot: "slowed",
                description: "slowed",
                buff: "488e53ede2802ff4da9372c6a494fb66",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Reflex,
                activation: ActivationType.onlyHit
            );
            //////////////////////////////////////////////////////////////////////////////////
            //                         Debuff onlyOnFirstHit                                //
            //////////////////////////////////////////////////////////////////////////////////
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t6").ToString(), DC = 33 }
                },
                name: "Frightening",
                nameRoot: "frightened",
                description: "frightened",
                buff: "f08a7239aa961f34c8301518e71d4cdf",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t6").ToString(), DC = 33 }
                },
                name: "Stunning",
                nameRoot: "stunned",
                description: "stunned",
                buff: "09d39b38bb7c6014394b6daced9bacd3",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t6").ToString(), DC = 33 }
                },
                name: "Dazing",
                nameRoot: "daze",
                description: "dazed",
                buff: "d2e35b870e4ac574d9873b36402487e5",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t6").ToString(), DC = 33 }
                },
                name: "Slumbering",
                nameRoot: "sleep",
                description: "asleep",
                buff: "c9937d7846aa9ae46991e9f298be644a",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t6").ToString(), DC = 33 }
                },
                name: "Paralyzing",
                nameRoot: "paralyzed",
                description: "paralyzed",
                buff: "4d5a2e4c34d24acca575c10003cf8fbc",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t6").ToString(), DC = 33 }
                },
                name: "Exhausting",
                nameRoot: "exhausted",
                description: "exhausted",
                buff: "46d1b9cc3d0fd36469a471b047d773a2",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t6").ToString(), DC = 33 }
                },
                name: "Nauseating",
                nameRoot: "nauseated",
                description: "nauseated",
                buff: "956331dba5125ef48afe41875a00ca0e",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t6").ToString(), DC = 33 }
                },
                name: "Toppling",
                nameRoot: "prone",
                description: "prone",
                buff: "24cf3deb078d3df4d92ba24b176bda97",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Reflex,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t6").ToString(), DC = 33 }
                },
                name: "Dominating",
                nameRoot: "domination",
                description: "dominated",
                buff: "c0f4e1c24c9cd334ca988ed1bd9d201f",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyOnFirstHit
            );

            //////////////////////////////////////////////////////////////////////////////////
            //                            Extra Damage                                      //
            //////////////////////////////////////////////////////////////////////////////////
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                name: "Flaming",
                nameRoot: "fire",
                description: "fire",
                prefab: PREFAB_FLAMING
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                name: "Frost",
                nameRoot: "cold",
                description: "cold",
                prefab: PREFAB_FROST
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                name: "Shock",
                nameRoot: "electricity",
                description: "electricity",
                prefab: PREFAB_SHOCK
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                name: "Thundering",
                nameRoot: "sonic",
                description: "sonic",
                prefab: PREFAB_SONIC
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                name: "Corrosive",
                nameRoot: "acid",
                description: "acid",
                prefab: PREFAB_CORROSIVE
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                name: "Unholy",
                nameRoot: "unholy",
                description: "negative damage",
                prefab: PREFAB_UNHOLY
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                name: "Holy",
                nameRoot: "holy",
                description: "holy",
                prefab: PREFAB_HOLY
            );


            //////////////////////////////////////////////////////////////////////////////////
            //                                Stats Bonus                                   //
            //////////////////////////////////////////////////////////////////////////////////
            
            //Base Stats
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t6").ToString(), Bonus = 6 }
                },
                name: "Mighty",
                nameRoot: "statSTR",
                description: "strength",
                stat: StatType.Strength
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t6").ToString(), Bonus = 6 }
                },
                name: "Graceful",
                nameRoot: "statDEX",
                description: "dexterity",
                stat: StatType.Dexterity
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t6").ToString(), Bonus = 6 }
                },
                name: "Resilient",
                nameRoot: "statCON",
                description: "constitution",
                stat: StatType.Constitution
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t6").ToString(), Bonus = 6 }
                },
                name: "Cunning",
                nameRoot: "statINT",
                description: "intelligence",
                stat: StatType.Intelligence
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t6").ToString(), Bonus = 6 }
                },
                name: "Sage",
                nameRoot: "statWIS",
                description: "wisdom",
                stat: StatType.Wisdom
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t6").ToString(), Bonus = 6 }
                },
                name: "Glamorous",
                nameRoot: "statCHA",
                description: "charisma",
                stat: StatType.Charisma
            );

            //Saves
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t6").ToString(), Bonus = 6 }
                },
                name: "Enduring",
                nameRoot: "saveFOR",
                description: "fortitude saving throw",
                stat: StatType.SaveFortitude
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t6").ToString(), Bonus = 6 }
                },
                name: "Ironwill",
                nameRoot: "saveWIL",
                description: "will saving throw",
                stat: StatType.SaveWill
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t6").ToString(), Bonus = 6 }
                },
                name: "Elusive",
                nameRoot: "saveREF",
                description: "reflex saving throw",
                stat: StatType.SaveReflex
            );

            //Skills
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t6").ToString(), Bonus = 12 }
                },
                name: "Mobile",
                nameRoot: "skillMOB",
                description: "mobility",
                stat: StatType.SkillMobility
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t6").ToString(), Bonus = 12 }
                },
                name: "Vigorous",
                nameRoot: "skillATH",
                description: "athletics",
                stat: StatType.SkillAthletics
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t6").ToString(), Bonus = 12 }
                },
                name: "Arcane",
                nameRoot: "skillARC",
                description: "knowledge arcana",
                stat: StatType.SkillKnowledgeArcana
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t6").ToString(), Bonus = 12 }
                },
                name: "Scholar",
                nameRoot: "skillWOR",
                description: "knowledge world",
                stat: StatType.SkillKnowledgeWorld
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t6").ToString(), Bonus = 12 }
                },
                name: "Pathfinder",
                nameRoot: "skillNAT",
                description: "lore nature",
                stat: StatType.SkillLoreNature
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t6").ToString(), Bonus = 12 }
                },
                name: "Saintly",
                nameRoot: "skillREL",
                description: "lore religion",
                stat: StatType.SkillLoreReligion
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t6").ToString(), Bonus = 12 }
                },
                name: "Vigilant",
                nameRoot: "skillPERC",
                description: "perception",
                stat: StatType.SkillPerception
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t6").ToString(), Bonus = 12 }
                },
                name: "Diplomatic",
                nameRoot: "skillPERS",
                description: "persuasion",
                stat: StatType.SkillPersuasion
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t6").ToString(), Bonus = 12 }
                },
                name: "Silent",
                nameRoot: "skillSTE",
                description: "stealth",
                stat: StatType.SkillStealth
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t6").ToString(), Bonus = 12 }
                },
                name: "Gambit",
                nameRoot: "skillTHI",
                description: "Trickery",
                stat: StatType.SkillThievery
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t6").ToString(), Bonus = 12 }
                },
                name: "Mystic",
                nameRoot: "skillUMD",
                description: "use magic device",
                stat: StatType.SkillUseMagicDevice
            );

            //others
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t6").ToString(), Bonus = 12 }
                },
                name: "Swift",
                nameRoot: "initiative",
                description: "initiative",
                stat: StatType.Initiative
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t6").ToString(), Bonus = 6 }
                },
                name: "Grappling",
                nameRoot: "CMB",
                description: "combat maneuver bonus",
                stat: StatType.AdditionalCMB
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t6").ToString(), Bonus = 6 }
                },
                name: "Immovable",
                nameRoot: "CMD",
                description: "combat maneuver defense",
                stat: StatType.AdditionalCMD
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t2").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t3").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t4").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t5").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t6").ToString(), Bonus = 3 }
                },
                name: "Eldritch",
                nameRoot: "casterLevel",
                description: "caster level",
                stat: StatType.BonusCasterLevel
            );

            //////////////////////////////////////////////////////////////////////////////////
            //                                  Caster                                      //
            //////////////////////////////////////////////////////////////////////////////////
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t1").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t2").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t3").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t4").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t1").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t5").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t6").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t3").ToString(), BonusDescription = 3 }
                },
                name: "Hexing",
                nameRoot: "spellDC",
                description: "spell DC for all saving trhow against spells from the wielder casts"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDieBonus.t1").ToString(), Feat = GuidUtil.FeatureGuid("spellDieBonus.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDieBonus.t2").ToString(), Feat = GuidUtil.FeatureGuid("spellDieBonus.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDieBonus.t3").ToString(), Feat = GuidUtil.FeatureGuid("spellDieBonus.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDieBonus.t4").ToString(), Feat = GuidUtil.FeatureGuid("spellDieBonus.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDieBonus.t5").ToString(), Feat = GuidUtil.FeatureGuid("spellDieBonus.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDieBonus.t6").ToString(), Feat = GuidUtil.FeatureGuid("spellDieBonus.t3").ToString(), BonusDescription = 3 }
                },
                name: "Overcharged",
                nameRoot: "spellDieBonus",
                description: "each die rolled when casting a spell with descriptor fire, cold, elctricity, acid, sonic, force and cure"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationCL.t1").ToString(), Feat = GuidUtil.FeatureGuid("divinationCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationCL.t2").ToString(), Feat = GuidUtil.FeatureGuid("divinationCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationCL.t3").ToString(), Feat = GuidUtil.FeatureGuid("divinationCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationCL.t4").ToString(), Feat = GuidUtil.FeatureGuid("divinationCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationCL.t5").ToString(), Feat = GuidUtil.FeatureGuid("divinationCL.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationCL.t6").ToString(), Feat = GuidUtil.FeatureGuid("divinationCL.t3").ToString(), BonusDescription = 3 }
                },
                name: "Revealing",
                nameRoot: "divinationCL",
                description: "caster level on divination school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentCL.t1").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentCL.t2").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentCL.t3").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentCL.t4").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentCL.t5").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentCL.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentCL.t6").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentCL.t3").ToString(), BonusDescription = 3 }
                },
                name: "Amplifier",
                nameRoot: "enchantmentCL",
                description: "caster level on enchantment school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationCL.t1").ToString(), Feat = GuidUtil.FeatureGuid("evocationCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationCL.t2").ToString(), Feat = GuidUtil.FeatureGuid("evocationCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationCL.t3").ToString(), Feat = GuidUtil.FeatureGuid("evocationCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationCL.t4").ToString(), Feat = GuidUtil.FeatureGuid("evocationCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationCL.t5").ToString(), Feat = GuidUtil.FeatureGuid("evocationCL.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationCL.t6").ToString(), Feat = GuidUtil.FeatureGuid("evocationCL.t3").ToString(), BonusDescription = 3 }
                },
                name: "Blasting",
                nameRoot: "evocationCL",
                description: "caster level on evocation school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationCL.t1").ToString(), Feat = GuidUtil.FeatureGuid("conjurationCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationCL.t2").ToString(), Feat = GuidUtil.FeatureGuid("conjurationCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationCL.t3").ToString(), Feat = GuidUtil.FeatureGuid("conjurationCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationCL.t4").ToString(), Feat = GuidUtil.FeatureGuid("conjurationCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationCL.t5").ToString(), Feat = GuidUtil.FeatureGuid("conjurationCL.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationCL.t6").ToString(), Feat = GuidUtil.FeatureGuid("conjurationCL.t3").ToString(), BonusDescription = 3 }
                },
                name: "Summoning",
                nameRoot: "conjurationCL",
                description: "caster level on conjuration school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationCL.t1").ToString(), Feat = GuidUtil.FeatureGuid("abjurationCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationCL.t2").ToString(), Feat = GuidUtil.FeatureGuid("abjurationCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationCL.t3").ToString(), Feat = GuidUtil.FeatureGuid("abjurationCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationCL.t4").ToString(), Feat = GuidUtil.FeatureGuid("abjurationCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationCL.t5").ToString(), Feat = GuidUtil.FeatureGuid("abjurationCL.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationCL.t6").ToString(), Feat = GuidUtil.FeatureGuid("abjurationCL.t3").ToString(), BonusDescription = 3 }
                },
                name: "Warding",
                nameRoot: "abjurationCL",
                description: "caster level on abjuration school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionCL.t1").ToString(), Feat = GuidUtil.FeatureGuid("illusionCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionCL.t2").ToString(), Feat = GuidUtil.FeatureGuid("illusionCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionCL.t3").ToString(), Feat = GuidUtil.FeatureGuid("illusionCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionCL.t4").ToString(), Feat = GuidUtil.FeatureGuid("illusionCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionCL.t5").ToString(), Feat = GuidUtil.FeatureGuid("illusionCL.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionCL.t6").ToString(), Feat = GuidUtil.FeatureGuid("illusionCL.t3").ToString(), BonusDescription = 3 }
                },
                name: "Mirage",
                nameRoot: "illusionCL",
                description: "caster level on illusion school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionCL.t1").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionCL.t2").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionCL.t3").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionCL.t4").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionCL.t5").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionCL.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionCL.t6").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionCL.t3").ToString(), BonusDescription = 3 }
                },
                name: "Morphing",
                nameRoot: "transmutacionCL",
                description: "caster level on transmutacion school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyCL.t1").ToString(), Feat = GuidUtil.FeatureGuid("necromancyCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyCL.t2").ToString(), Feat = GuidUtil.FeatureGuid("necromancyCL.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyCL.t3").ToString(), Feat = GuidUtil.FeatureGuid("necromancyCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyCL.t4").ToString(), Feat = GuidUtil.FeatureGuid("necromancyCL.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyCL.t5").ToString(), Feat = GuidUtil.FeatureGuid("necromancyCL.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyCL.t6").ToString(), Feat = GuidUtil.FeatureGuid("necromancyCL.t3").ToString(), BonusDescription = 3 }
                },
                name: "Grim",
                nameRoot: "necromancyCL",
                description: "caster level on necromancy school spells"
            );

            //SpellSchoolsDC
            //Bonus here its only for description
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationDC.t1").ToString(), Feat = GuidUtil.FeatureGuid("divinationDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationDC.t2").ToString(), Feat = GuidUtil.FeatureGuid("divinationDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationDC.t3").ToString(), Feat = GuidUtil.FeatureGuid("divinationDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationDC.t4").ToString(), Feat = GuidUtil.FeatureGuid("divinationDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationDC.t5").ToString(), Feat = GuidUtil.FeatureGuid("divinationDC.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("divinationDC.t6").ToString(), Feat = GuidUtil.FeatureGuid("divinationDC.t3").ToString(), BonusDescription = 3 }
                },
                name: "Insightful",
                nameRoot: "divinationDC",
                description: "DC on divination school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentDC.t1").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentDC.t2").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentDC.t3").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentDC.t4").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentDC.t5").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentDC.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("enchantmentDC.t6").ToString(), Feat = GuidUtil.FeatureGuid("enchantmentDC.t3").ToString(), BonusDescription = 3 }
                },
                name: "Mesmeric",
                nameRoot: "enchantmentDC",
                description: "DC on enchantment school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationDC.t1").ToString(), Feat = GuidUtil.FeatureGuid("evocationDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationDC.t2").ToString(), Feat = GuidUtil.FeatureGuid("evocationDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationDC.t3").ToString(), Feat = GuidUtil.FeatureGuid("evocationDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationDC.t4").ToString(), Feat = GuidUtil.FeatureGuid("evocationDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationDC.t5").ToString(), Feat = GuidUtil.FeatureGuid("evocationDC.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("evocationDC.t6").ToString(), Feat = GuidUtil.FeatureGuid("evocationDC.t3").ToString(), BonusDescription = 3 }
                },
                name: "Cataclysmic",
                nameRoot: "evocationDC",
                description: "DC on evocation school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationDC.t1").ToString(), Feat = GuidUtil.FeatureGuid("conjurationDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationDC.t2").ToString(), Feat = GuidUtil.FeatureGuid("conjurationDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationDC.t3").ToString(), Feat = GuidUtil.FeatureGuid("conjurationDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationDC.t4").ToString(), Feat = GuidUtil.FeatureGuid("conjurationDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationDC.t5").ToString(), Feat = GuidUtil.FeatureGuid("conjurationDC.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("conjurationDC.t6").ToString(), Feat = GuidUtil.FeatureGuid("conjurationDC.t3").ToString(), BonusDescription = 3 }
                },
                name: "Binding",
                nameRoot: "conjurationDC",
                description: "DC on conjuration school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationDC.t1").ToString(), Feat = GuidUtil.FeatureGuid("abjurationDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationDC.t2").ToString(), Feat = GuidUtil.FeatureGuid("abjurationDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationDC.t3").ToString(), Feat = GuidUtil.FeatureGuid("abjurationDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationDC.t4").ToString(), Feat = GuidUtil.FeatureGuid("abjurationDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationDC.t5").ToString(), Feat = GuidUtil.FeatureGuid("abjurationDC.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("abjurationDC.t6").ToString(), Feat = GuidUtil.FeatureGuid("abjurationDC.t3").ToString(), BonusDescription = 3 }
                },
                name: "Repelling",
                nameRoot: "abjurationDC",
                description: "DC on abjuration school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionDC.t1").ToString(), Feat = GuidUtil.FeatureGuid("illusionDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionDC.t2").ToString(), Feat = GuidUtil.FeatureGuid("illusionDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionDC.t3").ToString(), Feat = GuidUtil.FeatureGuid("illusionDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionDC.t4").ToString(), Feat = GuidUtil.FeatureGuid("illusionDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionDC.t5").ToString(), Feat = GuidUtil.FeatureGuid("illusionDC.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("illusionDC.t6").ToString(), Feat = GuidUtil.FeatureGuid("illusionDC.t3").ToString(), BonusDescription = 3 }
                },
                name: "Chimeric",
                nameRoot: "illusionDC",
                description: "DC on illusion school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionDC.t1").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionDC.t2").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionDC.t3").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionDC.t4").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionDC.t5").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionDC.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("transmutacionDC.t6").ToString(), Feat = GuidUtil.FeatureGuid("transmutacionDC.t3").ToString(), BonusDescription = 3 }
                },
                name: "Mutable",
                nameRoot: "transmutacionDC",
                description: "DC on transmutacion school spells"
            );
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyDC.t1").ToString(), Feat = GuidUtil.FeatureGuid("necromancyDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyDC.t2").ToString(), Feat = GuidUtil.FeatureGuid("necromancyDC.t1").ToString(), BonusDescription = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyDC.t3").ToString(), Feat = GuidUtil.FeatureGuid("necromancyDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyDC.t4").ToString(), Feat = GuidUtil.FeatureGuid("necromancyDC.t2").ToString(), BonusDescription = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyDC.t5").ToString(), Feat = GuidUtil.FeatureGuid("necromancyDC.t3").ToString(), BonusDescription = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("necromancyDC.t6").ToString(), Feat = GuidUtil.FeatureGuid("necromancyDC.t3").ToString(), BonusDescription = 3 }
                },
                name: "Deathly",
                nameRoot: "necromancyDC",
                description: "DC on necromancy school spells"
            );

            //Item Price
            EnchantFactory.RegisterWeaponPriceForTiers(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("price_20").ToString()},
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("price_40").ToString()},
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("price_80").ToString()},
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("price_160").ToString()},
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("price_320").ToString()},
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("price_640").ToString()},
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("price_1280").ToString()}
                }
            );
        }
    }
}
