using Kingmaker.ElementsSystem;
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
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("spellDC.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("spellDC.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("spellDC.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Spell DC"
            );
        }
    }
}
