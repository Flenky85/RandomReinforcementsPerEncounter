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
                    new DebuffTierConfig { Seed = "shaken.t1", DC = 13 },
                    new DebuffTierConfig { Seed = "shaken.t2", DC = 17 },
                    new DebuffTierConfig { Seed = "shaken.t3", DC = 21 },
                    new DebuffTierConfig { Seed = "shaken.t4", DC = 25 },
                    new DebuffTierConfig { Seed = "shaken.t5", DC = 29 },
                    new DebuffTierConfig { Seed = "shaken.t6", DC = 33 }
                },
                nameRoot: "Shaken",
                buff: "25ec6cb6ab1845c48a95f9c20b034220",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 1,
                activationType:1
            ); 
            EnchantFactory.RegisterDebuffTiersFor(
                new List<DebuffTierConfig>
                {
                    new DebuffTierConfig { Seed = "blind.t1", DC = 13 },
                    new DebuffTierConfig { Seed = "blind.t2", DC = 17 },
                    new DebuffTierConfig { Seed = "blind.t3", DC = 21 },
                    new DebuffTierConfig { Seed = "blind.t4", DC = 25 },
                    new DebuffTierConfig { Seed = "blind.t5", DC = 29 },
                    new DebuffTierConfig { Seed = "blind.t6", DC = 33 }
                },
                nameRoot: "Blind",
                buff: "0ec36e7596a4928489d2049e1e1c76a7",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 1,
                activationType: 1
            );/*
            DebuffEnchantFactory.RegisterDebuffTiersFor(
                seedRoot1: "blind.t1",
                seedRoot2: "blind.t2",
                seedRoot3: "blind.t3",
                seedRoot4: "blind.t4",
                seedRoot5: "blind.t5",
                seedRoot6: "blind.t6",
                DC1: "13",
                DC2: "17",
                DC3: "21",
                DC4: "25",
                DC5: "29",
                DC6: "33",
                nameRoot: "Blind",
                buff: "0ec36e7596a4928489d2049e1e1c76a7",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: 1,
                activationType: 1
            );*/
        }
    }
}
