using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomReinforcementsPerEncounter
{
    public static class EnchantList
    {
        public static readonly List<EnchantData> Item = new List<EnchantData> {
            new EnchantData
            {
                AssetIDT1 = new[] {
                    GuidUtil.EnchantGuid("shaken.t1").ToString(),
                    GuidUtil.EnchantGuid("blindness.t1").ToString(),
                    GuidUtil.EnchantGuid("dazzled.t1").ToString(),
                    GuidUtil.EnchantGuid("sickened.t1").ToString(),
                    GuidUtil.EnchantGuid("staggered.t1").ToString(),
                    GuidUtil.EnchantGuid("fatigue.t1").ToString(),
                    GuidUtil.EnchantGuid("confusion.t1").ToString(),
                    GuidUtil.EnchantGuid("entangled.t1").ToString(),
                    GuidUtil.EnchantGuid("slowed.t1").ToString(),
                },
                AssetIDT2 = new[] {
                    GuidUtil.EnchantGuid("shaken.t2").ToString(),
                    GuidUtil.EnchantGuid("blindness.t2").ToString(),
                    GuidUtil.EnchantGuid("dazzled.t2").ToString(),
                    GuidUtil.EnchantGuid("sickened.t2").ToString(),
                    GuidUtil.EnchantGuid("staggered.t2").ToString(),
                    GuidUtil.EnchantGuid("fatigue.t2").ToString(),
                    GuidUtil.EnchantGuid("confusion.t2").ToString(),
                    GuidUtil.EnchantGuid("entangled.t2").ToString(),
                    GuidUtil.EnchantGuid("slowed.t2").ToString(),
                },
                AssetIDT3 = new[] {
                    GuidUtil.EnchantGuid("shaken.t3").ToString(),
                    GuidUtil.EnchantGuid("blindness.t3").ToString(),
                    GuidUtil.EnchantGuid("dazzled.t3").ToString(),
                    GuidUtil.EnchantGuid("sickened.t3").ToString(),
                    GuidUtil.EnchantGuid("staggered.t3").ToString(),
                    GuidUtil.EnchantGuid("fatigue.t3").ToString(),
                    GuidUtil.EnchantGuid("confusion.t3").ToString(),
                    GuidUtil.EnchantGuid("entangled.t3").ToString(),
                    GuidUtil.EnchantGuid("slowed.t3").ToString(),
                },
                AssetIDT4 = new[] {
                    GuidUtil.EnchantGuid("shaken.t4").ToString(),
                    GuidUtil.EnchantGuid("blindness.t4").ToString(),
                    GuidUtil.EnchantGuid("dazzled.t4").ToString(),
                    GuidUtil.EnchantGuid("sickened.t4").ToString(),
                    GuidUtil.EnchantGuid("staggered.t4").ToString(),
                    GuidUtil.EnchantGuid("fatigue.t4").ToString(),
                    GuidUtil.EnchantGuid("confusion.t4").ToString(),
                    GuidUtil.EnchantGuid("entangled.t4").ToString(),
                    GuidUtil.EnchantGuid("slowed.t4").ToString(),
                },
                AssetIDT5 = new[] {
                    GuidUtil.EnchantGuid("shaken.t5").ToString(),
                    GuidUtil.EnchantGuid("blindness.t5").ToString(),
                    GuidUtil.EnchantGuid("dazzled.t5").ToString(),
                    GuidUtil.EnchantGuid("sickened.t5").ToString(),
                    GuidUtil.EnchantGuid("staggered.t5").ToString(),
                    GuidUtil.EnchantGuid("fatigue.t5").ToString(),
                    GuidUtil.EnchantGuid("confusion.t5").ToString(),
                    GuidUtil.EnchantGuid("entangled.t5").ToString(),
                    GuidUtil.EnchantGuid("slowed.t5").ToString(),
                },
                AssetIDT6 = new[] {
                    GuidUtil.EnchantGuid("shaken.t6").ToString(),
                    GuidUtil.EnchantGuid("blindness.t6").ToString(),
                    GuidUtil.EnchantGuid("dazzled.t6").ToString(),
                    GuidUtil.EnchantGuid("sickened.t6").ToString(),
                    GuidUtil.EnchantGuid("staggered.t6").ToString(),
                    GuidUtil.EnchantGuid("fatigue.t6").ToString(),
                    GuidUtil.EnchantGuid("confusion.t6").ToString(),
                    GuidUtil.EnchantGuid("entangled.t6").ToString(),
                    GuidUtil.EnchantGuid("slowed.t6").ToString(),
                },
                Value = 1,
                Type = EnchantType.OnHit,
            },
        };
    }
}
