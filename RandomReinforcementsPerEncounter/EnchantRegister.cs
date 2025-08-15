using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //savingThrowType: 1=fortitude, 2=reflex, 3=will
        //activationType: 1=onlyHit, 2=onlyOnFirstHit

        public static void RegisterAll()
        {
            //////////////////////////////////////////////////////////////////////////////////
            //                            Debuff onlyHit                                    //
            //////////////////////////////////////////////////////////////////////////////////
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "shaken.t1", DC = 11 },////////////////////////////
                    new DebuffTierConfig { Seed = "shaken.t2", DC = 14 },//                        //
                    new DebuffTierConfig { Seed = "shaken.t3", DC = 17 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "shaken.t4", DC = 20 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "shaken.t5", DC = 23 },//                        //
                    new DebuffTierConfig { Seed = "shaken.t6", DC = 26 } ////////////////////////////
                },
                nameRoot: "Shaken",
                description: "shaken",
                condition: "Shaken",
                buff: "20ec6cb6ab1845c48a95f9c20b034220",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 3,
                activationType:1
            ); 
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "blindness.t1", DC = 11 },////////////////////////////
                    new DebuffTierConfig { Seed = "blindness.t2", DC = 14 },//                        //
                    new DebuffTierConfig { Seed = "blindness.t3", DC = 17 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "blindness.t4", DC = 20 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "blindness.t5", DC = 23 },//                        //
                    new DebuffTierConfig { Seed = "blindness.t6", DC = 26 } ////////////////////////////
                },
                nameRoot: "Blindness",
                description:"blinded",
                condition: "Blind",
                buff: "0ec36e7596a4928489d2049e1e1c76a7",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 1,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "dazzled.t1", DC = 11 },////////////////////////////
                    new DebuffTierConfig { Seed = "dazzled.t2", DC = 14 },//                        //
                    new DebuffTierConfig { Seed = "dazzled.t3", DC = 17 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "dazzled.t4", DC = 20 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "dazzled.t5", DC = 23 },//                        //
                    new DebuffTierConfig { Seed = "dazzled.t6", DC = 26 } ////////////////////////////
                },
                nameRoot: "Dazzled",
                description: "dazzled",
                condition: "Dazzled",
                buff: "df6d1020da07524423afbae248845ecc",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 1,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "sickened.t1", DC = 11 },////////////////////////////
                    new DebuffTierConfig { Seed = "sickened.t2", DC = 14 },//                        //
                    new DebuffTierConfig { Seed = "sickened.t3", DC = 17 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "sickened.t4", DC = 20 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "sickened.t5", DC = 23 },//                        //
                    new DebuffTierConfig { Seed = "sickened.t6", DC = 26 } ////////////////////////////
                },
                nameRoot: "Sickened",
                description: "sickened",
                condition: "Sickened",
                buff: "4e42460798665fd4cb9143ffa7ada323",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: 1,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "staggered.t1", DC = 11 },////////////////////////////
                    new DebuffTierConfig { Seed = "staggered.t2", DC = 14 },//                        //
                    new DebuffTierConfig { Seed = "staggered.t3", DC = 17 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "staggered.t4", DC = 20 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "staggered.t5", DC = 23 },//                        //
                    new DebuffTierConfig { Seed = "staggered.t6", DC = 26 } ////////////////////////////
                },
                nameRoot: "Staggered",
                description: "staggered",
                condition: "Staggered",
                buff: "df3950af5a783bd4d91ab73eb8fa0fd3",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 1,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "fatigue.t1", DC = 11 },////////////////////////////
                    new DebuffTierConfig { Seed = "fatigue.t2", DC = 14 },//                        //
                    new DebuffTierConfig { Seed = "fatigue.t3", DC = 17 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "fatigue.t4", DC = 20 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "fatigue.t5", DC = 23 },//                        //
                    new DebuffTierConfig { Seed = "fatigue.t6", DC = 26 } ////////////////////////////
                },
                nameRoot: "Fatigue",
                description: "fatigued",
                condition: "Fatigued",
                buff: "e6f2fc5d73d88064583cb828801172f4",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 1,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "confusion.t1", DC = 11 },////////////////////////////
                    new DebuffTierConfig { Seed = "confusion.t2", DC = 14 },//                        //
                    new DebuffTierConfig { Seed = "confusion.t3", DC = 17 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "confusion.t4", DC = 20 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "confusion.t5", DC = 23 },//                        //
                    new DebuffTierConfig { Seed = "confusion.t6", DC = 26 } ////////////////////////////
                },
                nameRoot: "Confusion",
                description: "confused",
                condition: "Confusion",
                buff: "886c7407dc623dc499b9f1465ff382df",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: 3,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "entangled.t1", DC = 11 },////////////////////////////
                    new DebuffTierConfig { Seed = "entangled.t2", DC = 14 },//                        //
                    new DebuffTierConfig { Seed = "entangled.t3", DC = 17 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "entangled.t4", DC = 20 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "entangled.t5", DC = 23 },//                        //
                    new DebuffTierConfig { Seed = "entangled.t6", DC = 26 } ////////////////////////////
                },
                nameRoot: "Entangled",
                description: "entangled",
                condition: "Entangled",
                buff: "f7f6260726117cf4b90a6086b05d2e38",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: 2,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "slowed.t1", DC = 11 },////////////////////////////
                    new DebuffTierConfig { Seed = "slowed.t2", DC = 14 },//                        //
                    new DebuffTierConfig { Seed = "slowed.t3", DC = 17 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "slowed.t4", DC = 20 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "slowed.t5", DC = 23 },//                        //
                    new DebuffTierConfig { Seed = "slowed.t6", DC = 26 } ////////////////////////////
                },
                nameRoot: "Slowed",
                description: "slowed",
                condition: "Slowed",
                buff: "488e53ede2802ff4da9372c6a494fb66",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: 2,
                activationType: 1
            );
            //////////////////////////////////////////////////////////////////////////////////
            //                         Debuff onlyOnFirstHit                                //
            //////////////////////////////////////////////////////////////////////////////////
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "frightened.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "frightened.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "frightened.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "frightened.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "frightened.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "frightened.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Frightened",
                description: "frightened",
                condition: "Frightened",
                buff: "f08a7239aa961f34c8301518e71d4cdf",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 3,
                activationType: 2
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "stunned.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "stunned.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "stunned.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "stunned.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "stunned.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "stunned.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Stunned",
                description: "stunned",
                condition: "Stunned",
                buff: "09d39b38bb7c6014394b6daced9bacd3",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: 1,
                activationType: 2
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "daze.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "daze.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "daze.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "daze.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "daze.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "daze.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Daze",
                description: "dazed",
                condition: "Daze",
                buff: "d2e35b870e4ac574d9873b36402487e5",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 3,
                activationType: 2
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "sleep.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "sleep.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "sleep.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "sleep.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "sleep.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "sleep.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Sleep",
                description: "asleep",
                condition: "Sleep",
                buff: "c9937d7846aa9ae46991e9f298be644a",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 3,
                activationType: 2
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "paralyzed.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "paralyzed.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "paralyzed.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "paralyzed.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "paralyzed.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "paralyzed.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Paralyzed",
                description: "paralyzed",
                condition: "Paralyzed",
                buff: "4d5a2e4c34d24acca575c10003cf8fbc",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: 3,
                activationType: 2
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "exhausted.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "exhausted.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "exhausted.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "exhausted.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "exhausted.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "exhausted.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Exhausted",
                description: "exhausted",
                condition: "Exhausted",
                buff: "46d1b9cc3d0fd36469a471b047d773a2",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 1,
                activationType: 2
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "nauseated.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "nauseated.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "nauseated.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "nauseated.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "nauseated.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "nauseated.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Nauseated",
                description: "nauseated",
                condition: "Nauseated",
                buff: "956331dba5125ef48afe41875a00ca0e",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 1,
                activationType: 2
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "prone.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "prone.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "prone.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "prone.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "prone.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "prone.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Prone",
                description: "prone",
                condition: "Prone",
                buff: "24cf3deb078d3df4d92ba24b176bda97",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: 2,
                activationType: 2
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "domination.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "domination.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "domination.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "domination.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "domination.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "domination.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Domination",
                description: "dominated",
                condition: "Domination",
                buff: "c0f4e1c24c9cd334ca988ed1bd9d201f",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: 3,
                activationType: 2
            );

            //////////////////////////////////////////////////////////////////////////////////
            //                            Extra Damage                                      //
            //////////////////////////////////////////////////////////////////////////////////
            EnchantFactory.RegisterDamageTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "fire.t1", DiceCount = 1, DiceSide = 3 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "fire.t2", DiceCount = 1, DiceSide = 6 }, //                        //
                    new DebuffTierConfig { Seed = "fire.t3", DiceCount = 1, DiceSide = 10 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "fire.t4", DiceCount = 2, DiceSide = 6 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "fire.t5", DiceCount = 2, DiceSide = 8 }, //                        //
                    new DebuffTierConfig { Seed = "fire.t6", DiceCount = 2, DiceSide = 10 } ////////////////////////////
                },
                nameRoot: "Flaming",
                description: "fire",
                prefab: PREFAB_FLAMING
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "cold.t1", DiceCount = 1, DiceSide = 3 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "cold.t2", DiceCount = 1, DiceSide = 6 }, //                        //
                    new DebuffTierConfig { Seed = "cold.t3", DiceCount = 1, DiceSide = 10 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "cold.t4", DiceCount = 2, DiceSide = 6 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "cold.t5", DiceCount = 2, DiceSide = 8 }, //                        //
                    new DebuffTierConfig { Seed = "cold.t6", DiceCount = 2, DiceSide = 10 } ////////////////////////////
                },
                nameRoot: "Frost",
                description: "cold",
                prefab: PREFAB_FROST
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "electricity.t1", DiceCount = 1, DiceSide = 3 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "electricity.t2", DiceCount = 1, DiceSide = 6 }, //                        //
                    new DebuffTierConfig { Seed = "electricity.t3", DiceCount = 1, DiceSide = 10 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "electricity.t4", DiceCount = 2, DiceSide = 6 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "electricity.t5", DiceCount = 2, DiceSide = 8 }, //                        //
                    new DebuffTierConfig { Seed = "electricity.t6", DiceCount = 2, DiceSide = 10 } ////////////////////////////
                },
                nameRoot: "Shock",
                description: "electricity",
                prefab: PREFAB_SHOCK
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "sonic.t1", DiceCount = 1, DiceSide = 3 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "sonic.t2", DiceCount = 1, DiceSide = 6 }, //                        //
                    new DebuffTierConfig { Seed = "sonic.t3", DiceCount = 1, DiceSide = 10 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "sonic.t4", DiceCount = 2, DiceSide = 6 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "sonic.t5", DiceCount = 2, DiceSide = 8 }, //                        //
                    new DebuffTierConfig { Seed = "sonic.t6", DiceCount = 2, DiceSide = 10 } ////////////////////////////
                },
                nameRoot: "Thundering",
                description: "sonic",
                prefab: PREFAB_SONIC
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "acid.t1", DiceCount = 1, DiceSide = 3 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "acid.t2", DiceCount = 1, DiceSide = 6 }, //                        //
                    new DebuffTierConfig { Seed = "acid.t3", DiceCount = 1, DiceSide = 10 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "acid.t4", DiceCount = 2, DiceSide = 6 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "acid.t5", DiceCount = 2, DiceSide = 8 }, //                        //
                    new DebuffTierConfig { Seed = "acid.t6", DiceCount = 2, DiceSide = 10 } ////////////////////////////
                },
                nameRoot: "Corrosive",
                description: "acid",
                prefab: PREFAB_CORROSIVE
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "unholy.t1", DiceCount = 1, DiceSide = 3 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "unholy.t2", DiceCount = 1, DiceSide = 6 }, //                        //
                    new DebuffTierConfig { Seed = "unholy.t3", DiceCount = 1, DiceSide = 10 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "unholy.t4", DiceCount = 2, DiceSide = 6 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "unholy.t5", DiceCount = 2, DiceSide = 8 }, //                        //
                    new DebuffTierConfig { Seed = "unholy.t6", DiceCount = 2, DiceSide = 10 } ////////////////////////////
                },
                nameRoot: "Unholy",
                description: "negative damage",
                prefab: PREFAB_UNHOLY
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "holy.t1", DiceCount = 1, DiceSide = 3 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "holy.t2", DiceCount = 1, DiceSide = 6 }, //                        //
                    new DebuffTierConfig { Seed = "holy.t3", DiceCount = 1, DiceSide = 10 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "holy.t4", DiceCount = 2, DiceSide = 6 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "holy.t5", DiceCount = 2, DiceSide = 8 }, //                        //
                    new DebuffTierConfig { Seed = "holy.t6", DiceCount = 2, DiceSide = 10 } ////////////////////////////
                },
                nameRoot: "Holy",
                description: "holy",
                prefab: PREFAB_HOLY
            );


            //////////////////////////////////////////////////////////////////////////////////
            //                                Stats Bonus                                   //
            //////////////////////////////////////////////////////////////////////////////////
            
            //Base Stats
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "statSTR.t1", Bonus = 1 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "statSTR.t2", Bonus = 2 }, //                        //
                    new DebuffTierConfig { Seed = "statSTR.t3", Bonus = 3 }, //       Dont Touch       //
                    new DebuffTierConfig { Seed = "statSTR.t4", Bonus = 4 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "statSTR.t5", Bonus = 5 }, //                        //
                    new DebuffTierConfig { Seed = "statSTR.t6", Bonus = 6 }  ////////////////////////////
                },
                nameRoot: "Strength",
                description: "strength",
                encyclopedia: "Strength",
                stat: StatType.Strength
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "statDEX.t1", Bonus = 1 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "statDEX.t2", Bonus = 2 }, //                        //
                    new DebuffTierConfig { Seed = "statDEX.t3", Bonus = 3 }, //       Dont Touch       //
                    new DebuffTierConfig { Seed = "statDEX.t4", Bonus = 4 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "statDEX.t5", Bonus = 5 }, //                        //
                    new DebuffTierConfig { Seed = "statDEX.t6", Bonus = 6 }  ////////////////////////////
                },
                nameRoot: "Dexterity",
                description: "dexterity",
                encyclopedia: "Dexterity",
                stat: StatType.Dexterity
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "statCON.t1", Bonus = 1 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "statCON.t2", Bonus = 2 }, //                        //
                    new DebuffTierConfig { Seed = "statCON.t3", Bonus = 3 }, //       Dont Touch       //
                    new DebuffTierConfig { Seed = "statCON.t4", Bonus = 4 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "statCON.t5", Bonus = 5 }, //                        //
                    new DebuffTierConfig { Seed = "statCON.t6", Bonus = 6 }  ////////////////////////////
                },
                nameRoot: "Constitution",
                description: "constitution",
                encyclopedia: "Constitution",
                stat: StatType.Constitution
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "statINT.t1", Bonus = 1 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "statINT.t2", Bonus = 2 }, //                        //
                    new DebuffTierConfig { Seed = "statINT.t3", Bonus = 3 }, //       Dont Touch       //
                    new DebuffTierConfig { Seed = "statINT.t4", Bonus = 4 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "statINT.t5", Bonus = 5 }, //                        //
                    new DebuffTierConfig { Seed = "statINT.t6", Bonus = 6 }  ////////////////////////////
                },
                nameRoot: "Intelligence",
                description: "intelligence",
                encyclopedia: "Intelligence",
                stat: StatType.Intelligence
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "statWIS.t1", Bonus = 1 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "statWIS.t2", Bonus = 2 }, //                        //
                    new DebuffTierConfig { Seed = "statWIS.t3", Bonus = 3 }, //       Dont Touch       //
                    new DebuffTierConfig { Seed = "statWIS.t4", Bonus = 4 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "statWIS.t5", Bonus = 5 }, //                        //
                    new DebuffTierConfig { Seed = "statWIS.t6", Bonus = 6 }  ////////////////////////////
                },
                nameRoot: "Wisdom",
                description: "wisdom",
                encyclopedia: "Wisdom",
                stat: StatType.Wisdom
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "statCHA.t1", Bonus = 1 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "statCHA.t2", Bonus = 2 }, //                        //
                    new DebuffTierConfig { Seed = "statCHA.t3", Bonus = 3 }, //       Dont Touch       //
                    new DebuffTierConfig { Seed = "statCHA.t4", Bonus = 4 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "statCHA.t5", Bonus = 5 }, //                        //
                    new DebuffTierConfig { Seed = "statCHA.t6", Bonus = 6 }  ////////////////////////////
                },
                nameRoot: "Charisma",
                description: "charisma",
                encyclopedia: "Charisma",
                stat: StatType.Charisma
            );

            //Saves
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "saveFOR.t1", Bonus = 1 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "saveFOR.t2", Bonus = 2 }, //                        //
                    new DebuffTierConfig { Seed = "saveFOR.t3", Bonus = 3 }, //       Dont Touch       //
                    new DebuffTierConfig { Seed = "saveFOR.t4", Bonus = 4 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "saveFOR.t5", Bonus = 5 }, //                        //
                    new DebuffTierConfig { Seed = "saveFOR.t6", Bonus = 6 }  ////////////////////////////
                },
                nameRoot: "Fortitude",
                description: "saving throw fortitude",
                encyclopedia: "Saving_Throw",
                stat: StatType.SaveFortitude
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "saveWIL.t1", Bonus = 1 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "saveWIL.t2", Bonus = 2 }, //                        //
                    new DebuffTierConfig { Seed = "saveWIL.t3", Bonus = 3 }, //       Dont Touch       //
                    new DebuffTierConfig { Seed = "saveWIL.t4", Bonus = 4 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "saveWIL.t5", Bonus = 5 }, //                        //
                    new DebuffTierConfig { Seed = "saveWIL.t6", Bonus = 6 }  ////////////////////////////
                },
                nameRoot: "Will",
                description: "saving throw will",
                encyclopedia: "Saving_Throw",
                stat: StatType.SaveWill
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "saveREF.t1", Bonus = 1 }, ////////////////////////////
                    new DebuffTierConfig { Seed = "saveREF.t2", Bonus = 2 }, //                        //
                    new DebuffTierConfig { Seed = "saveREF.t3", Bonus = 3 }, //       Dont Touch       //
                    new DebuffTierConfig { Seed = "saveREF.t4", Bonus = 4 }, //     seed for GUID      //
                    new DebuffTierConfig { Seed = "saveREF.t5", Bonus = 5 }, //                        //
                    new DebuffTierConfig { Seed = "saveREF.t6", Bonus = 6 }  ////////////////////////////
                },
                nameRoot: "Reflex",
                description: "saving throw reflex",
                encyclopedia: "Saving_Throw",
                stat: StatType.SaveReflex
            );

            //Skills
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "skillMOB.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "skillMOB.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "skillMOB.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "skillMOB.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "skillMOB.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "skillMOB.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Mobility",
                description: "mobility",
                encyclopedia: "Mobility",
                stat: StatType.SkillMobility
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "skillATH.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "skillATH.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "skillATH.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "skillATH.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "skillATH.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "skillATH.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Athletics",
                description: "athletics",
                encyclopedia: "Athletics",
                stat: StatType.SkillAthletics
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "skillARC.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "skillARC.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "skillARC.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "skillARC.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "skillARC.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "skillARC.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Knowledge Arcana",
                description: "knowledge arcana",
                encyclopedia: "Knowledge_Arcana",
                stat: StatType.SkillKnowledgeArcana
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "skillWOR.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "skillWOR.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "skillWOR.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "skillWOR.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "skillWOR.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "skillWOR.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Knowledge World",
                description: "knowledge world",
                encyclopedia: "Knowledge_World",
                stat: StatType.SkillKnowledgeWorld
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "skillNAT.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "skillNAT.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "skillNAT.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "skillNAT.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "skillNAT.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "skillNAT.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Lore Nature",
                description: "lore nature",
                encyclopedia: "Lore_Nature",
                stat: StatType.SkillLoreNature
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "skillREL.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "skillREL.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "skillREL.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "skillREL.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "skillREL.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "skillREL.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Lore Religion",
                description: "lore religion",
                encyclopedia: "Lore_Religion",
                stat: StatType.SkillLoreReligion
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "skillPERC.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "skillPERC.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "skillPERC.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "skillPERC.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "skillPERC.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "skillPERC.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Perception",
                description: "perception",
                encyclopedia: "Perception",
                stat: StatType.SkillPerception
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "skillPERS.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "skillPERS.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "skillPERS.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "skillPERS.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "skillPERS.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "skillPERS.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Persuasion",
                description: "persuasion",
                encyclopedia: "Persuasion",
                stat: StatType.SkillPersuasion
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "skillSTE.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "skillSTE.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "skillSTE.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "skillSTE.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "skillSTE.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "skillSTE.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Stealth",
                description: "stealth",
                encyclopedia: "Stealth",
                stat: StatType.SkillStealth
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "skillTHI.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "skillTHI.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "skillTHI.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "skillTHI.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "skillTHI.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "skillTHI.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Trickery",
                description: "Trickery",
                encyclopedia: "Trickery",
                stat: StatType.SkillThievery
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "skillUMD.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "skillUMD.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "skillUMD.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "skillUMD.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "skillUMD.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "skillUMD.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Use Magic Device",
                description: "use magic device",
                encyclopedia: "Use_Magic_Device",
                stat: StatType.SkillUseMagicDevice
            );

            //others
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "initiative.t1", Bonus = 2 },   ////////////////////////////
                    new DebuffTierConfig { Seed = "initiative.t2", Bonus = 4 },   //                        //
                    new DebuffTierConfig { Seed = "initiative.t3", Bonus = 6 },   //       Dont Touch       //
                    new DebuffTierConfig { Seed = "initiative.t4", Bonus = 8 },   //     seed for GUID      //
                    new DebuffTierConfig { Seed = "initiative.t5", Bonus = 10 },  //                        //
                    new DebuffTierConfig { Seed = "initiative.t6", Bonus = 12 }   ////////////////////////////
                },
                nameRoot: "Initiative",
                description: "initiative",
                encyclopedia: "Initiative",
                stat: StatType.Initiative
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "CMB.t1", Bonus = 1 },  ////////////////////////////
                    new DebuffTierConfig { Seed = "CMB.t2", Bonus = 2 },  //                        //
                    new DebuffTierConfig { Seed = "CMB.t3", Bonus = 3 },  //       Dont Touch       //
                    new DebuffTierConfig { Seed = "CMB.t4", Bonus = 4 },  //     seed for GUID      //
                    new DebuffTierConfig { Seed = "CMB.t5", Bonus = 5 },  //                        //
                    new DebuffTierConfig { Seed = "CMB.t6", Bonus = 6 }   ////////////////////////////
                },
                nameRoot: "CMB",
                description: "combat maneuver bonus",
                encyclopedia: "CMB",
                stat: StatType.AdditionalCMB
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "CMD.t1", Bonus = 1 },  ////////////////////////////
                    new DebuffTierConfig { Seed = "CMD.t2", Bonus = 2 },  //                        //
                    new DebuffTierConfig { Seed = "CMD.t3", Bonus = 3 },  //       Dont Touch       //
                    new DebuffTierConfig { Seed = "CMD.t4", Bonus = 4 },  //     seed for GUID      //
                    new DebuffTierConfig { Seed = "CMD.t5", Bonus = 5 },  //                        //
                    new DebuffTierConfig { Seed = "CMD.t6", Bonus = 6 }   ////////////////////////////
                },
                nameRoot: "CMD",
                description: "combat maneuver defense",
                encyclopedia: "CMD",
                stat: StatType.AdditionalCMD
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "casterLevel.t1", Bonus = 1 },  ////////////////////////////
                    new DebuffTierConfig { Seed = "casterLevel.t2", Bonus = 1 },  //                        //
                    new DebuffTierConfig { Seed = "casterLevel.t3", Bonus = 1 },  //       Dont Touch       //
                    new DebuffTierConfig { Seed = "casterLevel.t4", Bonus = 2 },  //     seed for GUID      //
                    new DebuffTierConfig { Seed = "casterLevel.t5", Bonus = 2 },  //                        //
                    new DebuffTierConfig { Seed = "casterLevel.t6", Bonus = 2 }   ////////////////////////////
                },
                nameRoot: "Caster Level",
                description: "caster level",
                encyclopedia: "Caster_Level",
                stat: StatType.BonusCasterLevel
            );

            //////////////////////////////////////////////////////////////////////////////////
            //                                Caster Bonus                                  //
            //////////////////////////////////////////////////////////////////////////////////

            EnchantFactory.RegisterWeaponSpellsTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "spellDC.t1", Bonus = 1 },  ////////////////////////////
                    new DebuffTierConfig { Seed = "spellDC.t2", Bonus = 1 },  //                        //
                    new DebuffTierConfig { Seed = "spellDC.t3", Bonus = 2 },  //       Dont Touch       //
                    new DebuffTierConfig { Seed = "spellDC.t4", Bonus = 2 },  //     seed for GUID      //
                    new DebuffTierConfig { Seed = "spellDC.t5", Bonus = 3 },  //                        //
                    new DebuffTierConfig { Seed = "spellDC.t6", Bonus = 3 }   ////////////////////////////
                },
                nameRoot: "Spell DC",
                description: "spell DC",
                encyclopedia: "DC",
                type: "spellDC"
            );
        }
    }
}
