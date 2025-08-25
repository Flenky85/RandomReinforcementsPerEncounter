using System.Linq;
using RandomReinforcementsPerEncounter.Config.Ids.Generators;
using RandomReinforcementsPerEncounter.Domain.Models;
using static RandomReinforcementsPerEncounter.EnchantFactory; 

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
                                AssetId = EnchantIds.Id(def.Seed, t, grip),
                                Feat = FeatureIds.ForTier(def.Seed, featTier),
                                BonusDescription = featTier 
                            };
                        })
                        .ToList(),
                register: (tiers, grip) =>
                    RegisterWeaponFeaturesTiersFor(
                        tiers: tiers,
                        name: def.Name,                                
                        nameRoot: EnchantIds.RootWithHand(def.Seed, grip),        
                        description: def.Desc,                          
                        AffixDisplay: def.AffixDisplay,                
                        affix: def.Affix                               
                    ),
                lootType: def.Type
            );
        }
    }
}
