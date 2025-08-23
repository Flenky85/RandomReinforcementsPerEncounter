// EnchantBuilder.cs  (principal)
using Kingmaker.EntitySystem.Stats; // SavingThrowType
using RandomReinforcementsPerEncounter.Domain.Models;
using System.Linq;
using UnityEngine;
using static RandomReinforcementsPerEncounter.EnchantFactory; // Id, RootWithHand, RegisterDebuffTiersFor, RegisterDamageTiersFor

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments.Builder
{
    internal static partial class EnchantBuilder
    {
        internal static void Build(EnchantDef def)
        {
            switch (def.Type)
            {
                case EnchantType.OnHit:
                    Build_Debuff(def, ActivationType.onlyHit);
                    break;
                case EnchantType.OnlyOnFirstHit:
                    Build_Debuff(def, ActivationType.onlyOnFirstHit);
                    break;
                case EnchantType.EnergyDamage:
                    Build_EnergyDamage(def);
                    break;
                case EnchantType.StatsBonus:
                case EnchantType.SkillsBonus:   
                case EnchantType.Others:       
                case EnchantType.Caster:
                    Build_Bonus(def);
                    break;
                case EnchantType.CasterFeature:
                case EnchantType.SchoolCL:
                case EnchantType.SchoolDC:
                    Build_Features(def);
                    break;
                default:
                    throw new System.NotSupportedException($"EnchantType {def.Type} not implemented yet.");
            }
        }

        // Helper común 1H / 2H
        private static void BuildTiersForBothHands(
            EnchantDef def,
            System.Func<WeaponGrip, System.Collections.Generic.List<EnchantTierConfig>> makeTiers,
            System.Action<System.Collections.Generic.List<EnchantTierConfig>, WeaponGrip> register,
            EnchantType lootType)
        {
            foreach (var grip in new[] { WeaponGrip.OneHanded, WeaponGrip.TwoHanded })
            {
                var tiers = makeTiers(grip);
                register(tiers, grip);
                LootBuckets.AddRootVariant(lootType, def.Seed, grip, def.Affix, def.Chance);
            }
        }
    }
}
