using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.View.MapObjects;
using RandomReinforcementsPerEncounter.Config;
using RandomReinforcementsPerEncounter.Config.Settings;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    public static class LootPicker
    {
        public static void AddPickedWeaponToLoot(InteractionLootPart lootPart, int cr)
        {
            if (lootPart == null) return;

            var pick = WeaponRoller.PickRandomWeapon();
            if (string.IsNullOrEmpty(pick.AssetId)) return;

            var bp = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(pick.AssetId);
            var bpWeapon = bp as BlueprintItemWeapon;
            if (bpWeapon == null) return;

            var entity = bpWeapon.CreateEntity() as ItemEntityWeapon;
            if (entity == null) return;
            entity.Identify();

            if (UnityEngine.Random.value < (ModSettings.Instance.QualityMaterialPct / 100f))
            {
                float r = UnityEngine.Random.value * 100f;
                float c0 = ModSettings.Instance.MatColdIron;
                float c1 = c0 + ModSettings.Instance.MatMithral;
                float c2 = c1 + ModSettings.Instance.MatAdamantite;

                var matGuid =
                    (r < c0) ? LootRefs.ColdIron :
                    (r < c1) ? LootRefs.Mithral :
                    (r < c2) ? LootRefs.Adamantine :
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
                if (UnityEngine.Random.value < (ModSettings.Instance.CompositePct / 100f))
                {
                    EnchantApplier.AddEnchants(
                        entity,
                        true,
                        ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(LootRefs.Composite),
                        PriceRefs.PriceT1
                    );
                }
            }

            if (UnityEngine.Random.value < (ModSettings.Instance.MagicPct / 100f))
            {
                int[] chances = TierChances.CalcTierChances(cr);
                int tier = TierChances.GetRandomTier(chances);
                EnchantApplier.ApplyRandomTierEnchant(entity, tier, chances);
            }
            else if (UnityEngine.Random.value < (ModSettings.Instance.MasterworkPct / 100f))
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