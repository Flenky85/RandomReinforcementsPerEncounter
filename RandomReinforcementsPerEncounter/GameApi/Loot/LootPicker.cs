using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Persistence.JsonUtility;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.View.MapObjects;
using RandomReinforcementsPerEncounter.Config;
using System;
using System.Linq;
using UnityEngine;

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

            if (UnityEngine.Random.value < 0.20f)
            {
                int[] chances = TierChances.CalcTierChances(cr);
                for (int i = 0; i < chances.Length; i++)
                    Debug.Log($"Tier {i + 1}: {chances[i]}");

                int tier = TierChances.GetRandomTier(chances);

                EnchantApplier.ApplyRandomTierEnchant(entity, tier, chances);
            }

            if (bpWeapon.IsMagic == false)
            {
                if (UnityEngine.Random.value < 0.30f)
                {
                    var masterworkEnchant = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(LootRefs.MasterWork);
                    if (masterworkEnchant != null)
                    {
                        EnchantApplier.AddEnchants(
                            entity,
                            true,
                            masterworkEnchant,
                            PriceRefs.PriceT1
                        );
                        Debug.Log("[RRE] Masterwork enchant aplicado (arma no mágica).");
                    }
                }
            }

            lootPart.Loot.Add(entity); // mete la instancia en el cofre
        }
    }
}