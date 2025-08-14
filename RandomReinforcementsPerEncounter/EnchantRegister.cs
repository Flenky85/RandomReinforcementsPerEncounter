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
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "shaken.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "shaken.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "shaken.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "shaken.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "shaken.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "shaken.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Shaken",
                description: "shaken",
                condition: "Shaken",
                buff: "25ec6cb6ab1845c48a95f9c20b034220",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 3,
                activationType:1
            ); 
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "blindness.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "blindness.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "blindness.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "blindness.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "blindness.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "blindness.t6", DC = 33 } ////////////////////////////
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
                    new DebuffTierConfig { Seed = "dazzled.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "dazzled.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "dazzled.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "dazzled.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "dazzled.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "dazzled.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Dazzled",
                description: "dazzled",
                condition: "Dazzled",
                buff: "df6d1025da07524429afbae248845ecc",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 1,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "sickened.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "sickened.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "sickened.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "sickened.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "sickened.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "sickened.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Sickened",
                description: "sickened",
                condition: "Sickened",
                buff: "4e42460798665fd4cb9173ffa7ada323",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: 1,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "staggered.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "staggered.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "staggered.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "staggered.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "staggered.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "staggered.t6", DC = 33 } ////////////////////////////
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
                    new DebuffTierConfig { Seed = "fatigue.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "fatigue.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "fatigue.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "fatigue.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "fatigue.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "fatigue.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Fatigue",
                description: "fatigued",
                condition: "Fatigued",
                buff: "e6f2fc5d73d88064583cb828801212f4",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 1,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "confusion.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "confusion.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "confusion.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "confusion.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "confusion.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "confusion.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Confusion",
                description: "confused",
                condition: "Confusion",
                buff: "886c7407dc629dc499b9f1465ff382df",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: 3,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "entangled.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "entangled.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "entangled.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "entangled.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "entangled.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "entangled.t6", DC = 33 } ////////////////////////////
                },
                nameRoot: "Entangled",
                description: "entangled",
                condition: "Entangled",
                buff: "f7f6330726121cf4b90a6086b05d2e38",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: 2,
                activationType: 1
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "slowed.t1", DC = 13 },////////////////////////////
                    new DebuffTierConfig { Seed = "slowed.t2", DC = 17 },//                        //
                    new DebuffTierConfig { Seed = "slowed.t3", DC = 21 },//       Dont Touch       //
                    new DebuffTierConfig { Seed = "slowed.t4", DC = 25 },//     seed for GUID      //
                    new DebuffTierConfig { Seed = "slowed.t5", DC = 29 },//                        //
                    new DebuffTierConfig { Seed = "slowed.t6", DC = 33 } ////////////////////////////
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
        }
    }
}
