using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.View.MapObjects;
using RandomReinforcementsPerEncounter.Config;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    public static class LootPicker
    {
        public static void AddPickedWeaponToLoot(InteractionLootPart lootPart, int cr)
        {
            if (lootPart == null) return;

            var pick = WeaponRoller.PickRandomWeapon(WeaponLootBalance.OversizedChance);
            if (string.IsNullOrEmpty(pick.AssetId)) return;

            var bp = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(pick.AssetId);
            var bpWeapon = bp as BlueprintItemWeapon;
            if (bpWeapon == null) return;

            var entity = bpWeapon.CreateEntity() as ItemEntityWeapon;
            if (entity == null) return;
            entity.Identify();

            if (UnityEngine.Random.value < 0.20f)
            {
                float r = UnityEngine.Random.value;
                var matGuid =
                    (r < 0.50f) ? LootRefs.ColdIron :
                    (r < 0.80f) ? LootRefs.Mithral :
                    (r < 0.90f) ? LootRefs.Adamantine :
                                  LootRefs.Druchite;

                EnchantApplier.AddEnchants(
                    entity,
                    true,
                    ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(matGuid),
                    PriceRefs.PriceT1
                );

            }

            if (bpWeapon.Category == WeaponCategory.Longbow || bpWeapon.Category == WeaponCategory.Shortbow)
            {
                if (UnityEngine.Random.value < 0.50f)
                {
                    EnchantApplier.AddEnchants(
                        entity,
                        true,
                        ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(LootRefs.Composite),
                        PriceRefs.PriceT1
                    );
                }
            }

            if (UnityEngine.Random.value < 0.05f)
            {
                int[] chances = TierChances.CalcTierChances(cr);

                int tier = TierChances.GetRandomTier(chances);

                EnchantApplier.ApplyRandomTierEnchant(entity, tier, chances);
            } else if (UnityEngine.Random.value < 0.30f)
            {
                EnchantApplier.AddEnchants(
                    entity,
                    true,
                    ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(LootRefs.MasterWork),
                    PriceRefs.PriceT1
                );
            }

            lootPart.Loot.Add(entity); 
        }
    }
}