using System.Linq;
using RandomReinforcementsPerEncounter.Domain.Models;
using static RandomReinforcementsPerEncounter.EnchantFactory; // Id, RootWithHand, Feature

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments.Builder
{
    internal static partial class EnchantBuilder
    {
        private static void Build_Features(EnchantDef def)
        {
            BuildTiersForBothHands(
                def,
                makeTiers: grip =>
                    Enumerable.Range(1, 6)
                        .Select(t =>
                        {
                            int featTier = (grip == WeaponGrip.OneHanded)
                                ? def.TierMapOneHanded[t - 1]
                                : def.TierMapTwoHanded[t - 1];

                            return new EnchantTierConfig
                            {
                                AssetId = Id(def.Seed, t, grip),
                                Feat = Feature(def.Seed, featTier),
                                BonusDescription = featTier // <- en int, sin MapBonusDesc
                            };
                        })
                        .ToList(),
                register: (tiers, grip) =>
                    RegisterWeaponFeaturesTiersFor(
                        tiers: tiers,
                        name: def.Name,                                 // "Hexing"
                        nameRoot: RootWithHand(def.Seed, grip),         // raíz 1H/2H
                        description: def.Desc,                          // "spell DC..."
                        AffixDisplay: def.AffixDisplay,                 // texto afijo
                        affix: def.Affix                                // Prefix/Suffix
                    ),
                lootType: def.Type
            );
        }
    }
}
