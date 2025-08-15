﻿using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RandomReinforcementsPerEncounter.FeatureFactory;

namespace RandomReinforcementsPerEncounter
{
    internal class FeatureRegister
    {
        public static void RegisterAll()
        {
            FeatureFactory.RegisterSpellsDCTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { Seed = "spellDC.t1", Bonus = 1 },
                    new FeatureTierConfig { Seed = "spellDC.t2", Bonus = 2 },
                    new FeatureTierConfig { Seed = "spellDC.t3", Bonus = 3 }
                },
                nameRoot: "Spell DC",
                type: "spellDC"
            );
        }
    }
}
