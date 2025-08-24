using RandomReinforcementsPerEncounter.Config.Ids.Generators;
using RandomReinforcementsPerEncounter.Domain.Models;
using System.Linq;
using static RandomReinforcementsPerEncounter.EnchantFactory;

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments.Builder
{
    internal static partial class EnchantBuilder
    {
        private static void Build_EnergyDamage(EnchantDef def)
        {
            BuildTiersForBothHands(
                def,
                makeTiers: grip =>
                    Enumerable.Range(1, 6)
                        .Select(t =>
                        {
                            var (dice, sides) = (grip == WeaponGrip.OneHanded
                                ? def.DamageMapOneHanded[t - 1]
                                : def.DamageMapTwoHanded[t - 1]);
                            return new EnchantTierConfig
                            {
                                AssetId = EnchantIds.Id(def.Seed, t, grip),
                                DiceCount = dice,
                                DiceSide = sides
                            };
                        })
                        .ToList(),
                register: (tiers, grip) =>
                    RegisterDamageTiersFor(
                        tiers: tiers,
                        name: def.AffixDisplay,
                        nameRoot: EnchantIds.RootWithHand(def.Seed, grip),
                        description: def.Desc,
                        prefab: def.DamagePrefab,
                        affix: def.AffixDisplay
                    ),
                lootType: def.Type
            );
        }
    }
}
