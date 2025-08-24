// EnchantBuilder.Debuff.cs
using Kingmaker.EntitySystem.Stats; // SavingThrowType
using RandomReinforcementsPerEncounter.Config.Ids.Generators;
using RandomReinforcementsPerEncounter.Domain.Models;
using System.Linq;
using static RandomReinforcementsPerEncounter.EnchantFactory;

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments.Builder
{
    internal static partial class EnchantBuilder
    {
        private static void Build_Debuff(EnchantDef def, ActivationType activation)
        {
            BuildTiersForBothHands(
                def,
                makeTiers: grip =>
                    Enumerable.Range(1, 6)
                        .Select(t => new EnchantTierConfig
                        {
                            AssetId = EnchantIds.Id(def.Seed, t, grip),
                            DC = (grip == WeaponGrip.OneHanded
                                ? def.TierMapOneHanded[t - 1]
                                : def.TierMapTwoHanded[t - 1])
                        })
                        .ToList(),
                register: (tiers, grip) =>
                    RegisterDebuffTiersFor(
                        tiers: tiers,
                        name: def.AffixDisplay,
                        nameRoot: EnchantIds.RootWithHand(def.Seed, grip),
                        buff: def.OnHitBuffBlueprintId,
                        durationDiceCount: def.OnHitDurDiceCount ?? 1,
                        durationDiceSides: def.OnHitDurDiceSides ?? 1,
                        savingThrowType: def.OnHitSave ?? SavingThrowType.Will,
                        activation: activation,
                        description: def.Desc,
                        affix: def.AffixDisplay
                    ),
                lootType: def.Type
            );
        }
    }
}
