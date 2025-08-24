using RandomReinforcementsPerEncounter.Domain.Models;
using System.Linq;
using static RandomReinforcementsPerEncounter.EnchantFactory;
using RandomReinforcementsPerEncounter.Config.Ids.Generators;

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments.Builder
{
    internal static partial class EnchantBuilder
    {
        private static void Build_Bonus(EnchantDef def)
        {
            BuildTiersForBothHands(
                def,
                makeTiers: grip =>
                    Enumerable.Range(1, 6)
                        .Select(t => new EnchantTierConfig
                        {
                            AssetId = EnchantIds.Id(def.Seed, t, grip),
                            Bonus = (grip == WeaponGrip.OneHanded
                                ? def.TierMapOneHanded[t - 1]
                                : def.TierMapTwoHanded[t - 1])
                        })
                        .ToList(),
                register: (tiers, grip) =>
                    RegisterWeaponStatsTiersFor(
                        tiers: tiers,
                        name: def.Name,
                        nameRoot: EnchantIds.RootWithHand(def.Seed, grip),
                        stat: def.Stat,
                        description: def.Desc,
                        AffixDisplay: def.AffixDisplay,
                        affix: def.Affix
                    ),
                lootType: def.Type
            );
        }
    }
}
