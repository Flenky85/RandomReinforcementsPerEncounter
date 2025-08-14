using Kingmaker.ElementsSystem;
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
        //savingThrowType: 1=fortitude, 2=reflex, 3=will
        //activationType: 1=onlyHit, 2=onlyOnFirstHit
        public static void RegisterAll()
        {   
            //Debuff onlyHit
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

            //Debuff onlyOnFirstHit
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
        }
    }
}
