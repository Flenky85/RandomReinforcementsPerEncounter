using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Loot;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.View.MapObjects;
using RandomReinforcementsPerEncounter.Config;
using RandomReinforcementsPerEncounter.Config.Ids;
using RandomReinforcementsPerEncounter.State;
using System.Collections.Generic;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.GameApi.Chest
{
    internal static class ChestService
    {
        private static readonly BlueprintLoot _dummyLootBlueprint =
            ResourcesLibrary.TryGetBlueprint<BlueprintLoot>(BlueprintGuids.Loot.DummyLootTable);
        private static readonly BlueprintItem _goldItemBlueprint =
            ResourcesLibrary.TryGetBlueprint<BlueprintItem>(BlueprintGuids.Loot.GoldItem);
       
        private static readonly AccessTools.FieldRef<InteractionLootPart, bool> _destroyWhenEmptyRef =
            AccessTools.FieldRefAccess<InteractionLootPart, bool>("m_DestroyWhenEmpty");
        private static readonly AccessTools.FieldRef<InteractionLootSettings, BlueprintLootReference[]> _lootTablesRef =
            AccessTools.FieldRefAccess<InteractionLootSettings, BlueprintLootReference[]>("m_LootTables");

        internal sealed class PurchaseRule
        {
            public IEnumerable<ItemData> Items { get; }
            public float ChancePercent { get; }
            public bool BestHealing { get; }

            public PurchaseRule(IEnumerable<ItemData> items, float chancePercent, bool bestHealing = false)
            {
                Items = items;
                ChancePercent = chancePercent;
                BestHealing = bestHealing;
            }
        }

        private static readonly PurchaseRule[] PurchasePlan =
        {
            new PurchaseRule(PermanetBuffsItemsList.Item,      PurchaseChances.PermanentBuffs),
            new PurchaseRule(null,                             PurchaseChances.BestHealingPotion, bestHealing: true),
            new PurchaseRule(PotionsItemsList.Item,            PurchaseChances.Potions),
            new PurchaseRule(ConsumiblesList.Item,             PurchaseChances.Consumables),
            new PurchaseRule(PurifyingSolutionItemsList.Item,  PurchaseChances.PurifyingSolution),
            new PurchaseRule(CookingIngredientsItemsList.Item, PurchaseChances.CookingIngredients),
            new PurchaseRule(CampingCraftItemsList.Item,       PurchaseChances.CampingCraft),
            new PurchaseRule(CookingRecipesItemsList.Item,     PurchaseChances.CookingRecipes),
            new PurchaseRule(CraftItemsList.Item,              PurchaseChances.Craft),
            new PurchaseRule(CraftingSetsItemsList.Item,       PurchaseChances.CraftingSets),
            new PurchaseRule(QuiversItemsList.Item,            PurchaseChances.Quivers),
            new PurchaseRule(ScrollsItemsList.Item,            PurchaseChances.Scrolls),
            new PurchaseRule(UtilityItemsList.Item,            PurchaseChances.Utility),
            new PurchaseRule(TrashList.Item,                   PurchaseChances.Trash),
        };



        private static int RollBiasedGold(int baseGold)
        {
            // Sesgo cuadrático (más prob. de valores altos)
            float r = Random.value;
            float bias = 1f + 9f * Mathf.Pow(r, 2f);
            int finalGold = Mathf.RoundToInt(baseGold * bias);
            return Mathf.Max(1, finalGold);
        }


        // Wrapper null-safe para el handler
        internal static void TrySpawnDefaultChestAt(Vector3? maybePosition)
        {
            if (!maybePosition.HasValue) return;
            SpawnLootChest(BlueprintGuids.Chests.DefaultLootChest, maybePosition.Value);
    
        }

        internal static void SpawnLootChest(string chestBlueprintGuid, Vector3 position)
        {
            var chestBlueprint = ResourcesLibrary.TryGetBlueprint<BlueprintDynamicMapObject>(chestBlueprintGuid);
            if (chestBlueprint?.Prefab == null) return;

            var mapEntityData = Game.Instance.EntityCreator.SpawnMapObject(
                chestBlueprint,
                position,
                Quaternion.identity,
                Game.Instance.State.LoadedAreaState.MainState
            );

            var view = mapEntityData?.View as MapObjectView;
            if (view == null) return;

            var entityData = view.Data as MapObjectEntityData;
            if (entityData == null) return;

            var lootPart = entityData.Parts.Get<InteractionLootPart>() ?? entityData.Parts.Add<InteractionLootPart>();

            // En vez de reflection:
            _destroyWhenEmptyRef(lootPart) = true;
            lootPart.AlreadyUnlocked = true;

            var interactionLoot = view.gameObject.GetComponent<InteractionLoot>() ?? view.gameObject.AddComponent<InteractionLoot>();

            // Dummy loot table (la que usabas)
            if (_dummyLootBlueprint == null || interactionLoot.Settings == null) return;

            // En vez de reflection:
            _lootTablesRef(interactionLoot.Settings) = new[]
            {
                _dummyLootBlueprint.ToReference<BlueprintLootReference>()
            };

            // Genera el contenido
            foreach (var cr in LootContext.EnemyCRs)
            {
                AddLootFromEnemyCRs(lootPart, cr);
            }
            
            ChestVisuals.ApplyScaleFromLoot(view, lootPart);
        }

        private static void AddLootFromEnemyCRs(InteractionLootPart lootPart, int cr)
        {
            if (_goldItemBlueprint == null) return;

            int totalGold = 0;
            var entries = new List<LootEntry>(8);

            int baseGold = Config.LootEconomy.GetBaseGoldForCR(cr);
            totalGold += RollBiasedGold(baseGold); // usa el helper nuevo

            // Compras según plan
            foreach (var rule in PurchasePlan)
            {
                if (rule.BestHealing)
                    TryBuyBestHealingPotionInto(entries, ref totalGold, rule.ChancePercent);
                else
                    TryBuyRandomItemInto(entries, rule.Items, ref totalGold, rule.ChancePercent);
            }

            // Oro restante → convertir a ítem oro (escala 1:10 como ya hacías)
            entries.Add(new LootEntry
            {
                Item = _goldItemBlueprint.ToReference<BlueprintItemReference>(),
                Count = Mathf.Max(1, totalGold / LootEconomy.GoldToItemFactor)
            });

            if (entries.Count > 0)
                lootPart.AddItems(entries);

            if (Random.value < LootEconomy.WeaponBonusChance)
            {
                LootPicker.AddPickedWeaponToLoot(lootPart, cr);
            }
        }


        private static void TryBuyBestHealingPotionInto(List<LootEntry> entries, ref int totalGold, float chancePercent)
        {
            float chance = Mathf.Clamp01(chancePercent / 100f);
            if (Random.value >= chance) return;

            var candidates = new List<ItemData>();
            int maxCost = 0;

            foreach (var it in PotionshealsItemsList.Item)
            {
                if (!int.TryParse(it.Cost, out int cost)) continue;
                if (cost > totalGold) continue;

                if (cost > maxCost) { maxCost = cost; candidates.Clear(); candidates.Add(it); }
                else if (cost == maxCost) { candidates.Add(it); }
            }

            if (candidates.Count == 0) return;

            var best = candidates[Random.Range(0, candidates.Count)];
            var potionBp = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(best.AssetId);
            if (potionBp == null) return;

            entries.Add(new LootEntry { Item = potionBp.ToReference<BlueprintItemReference>(), Count = 1 });
            totalGold -= maxCost;
        }

        private static void TryBuyRandomItemInto(List<LootEntry> entries,
                                                 IEnumerable<ItemData> items,
                                                 ref int totalGold,
                                                 float chancePercent)
        {
            float chance = Mathf.Clamp01(chancePercent / 100f);
            if (Random.value >= chance) return;

            var candidates = new List<(ItemData data, int cost)>();
            foreach (var it in items)
            {
                if (!int.TryParse(it.Cost, out int cost)) continue;
                if (cost <= totalGold) candidates.Add((it, cost));
            }

            if (candidates.Count == 0) return;

            var pick = candidates[Random.Range(0, candidates.Count)];
            var bp = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(pick.data.AssetId);
            if (bp == null) return;

            entries.Add(new LootEntry { Item = bp.ToReference<BlueprintItemReference>(), Count = 1 });
            totalGold -= pick.cost;
        }
    }
}
