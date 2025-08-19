using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments; 
using Kingmaker.Blueprints.Loot;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.View.MapObjects;
using RandomReinforcementsPerEncounter.State;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.GameApi
{
    internal static class ChestService
    {
        private static readonly BlueprintLoot _dummyLootBlueprint =
            ResourcesLibrary.TryGetBlueprint<BlueprintLoot>(Config.LootIds.DummyLootTable);
        private static readonly BlueprintItem _goldItemBlueprint =
            ResourcesLibrary.TryGetBlueprint<BlueprintItem>(Config.LootIds.GoldItem);
       
        private static readonly AccessTools.FieldRef<InteractionLootPart, bool> _destroyWhenEmptyRef =
            AccessTools.FieldRefAccess<InteractionLootPart, bool>("m_DestroyWhenEmpty");
        private static readonly AccessTools.FieldRef<InteractionLootSettings, BlueprintLootReference[]> _lootTablesRef =
            AccessTools.FieldRefAccess<InteractionLootSettings, BlueprintLootReference[]>("m_LootTables");

        // Wrapper null-safe para el handler
        internal static void TrySpawnDefaultChestAt(Vector3? maybePosition)
        {
            if (!maybePosition.HasValue) return;
            SpawnLootChest(Config.ChestIds.DefaultLootChest, maybePosition.Value);
    
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

            // Escala visual del cofre según el +X más alto
            int maxPlus =
                lootPart?.Loot?.Items?
                    .OfType<ItemEntityWeapon>()
                    .Select(EnhancementLookup.GetWeaponEnhancementPlus)
                    .DefaultIfEmpty(0)
                    .Max() ?? 0;

            float s = ChestScale.ForPlus(maxPlus);
            view.transform.localScale = new Vector3(s, s, s);
        }

        private static void AddLootFromEnemyCRs(InteractionLootPart lootPart, int cr)
        {
            if (_goldItemBlueprint == null) return;

            int totalGold = 0;
            var entries = new List<LootEntry>(8);

            int baseGold = Config.LootEconomy.GetBaseGoldForCR(cr);

            // Sesgo cuadrático (más prob. de valores altos)
            float r = UnityEngine.Random.value;
            float bias = 1f + 9f * Mathf.Pow(r, 2f);
            int finalGold = Mathf.RoundToInt(baseGold * bias);

            totalGold += Mathf.Max(1, finalGold);

            // Compras “con probabilidad” y límite por oro disponible
            TryBuyRandomItemInto(entries, PermanetBuffsItemsList.Item, ref totalGold, 0.1f);
            TryBuyBestHealingPotionInto(entries, ref totalGold, 10f);
            TryBuyRandomItemInto(entries, PotionsItemsList.Item, ref totalGold, 5f);
            TryBuyRandomItemInto(entries, ConsumiblesList.Item, ref totalGold, 2.5f);
            TryBuyRandomItemInto(entries, PurifyingSolutionItemsList.Item, ref totalGold, 5f);
            TryBuyRandomItemInto(entries, CookingIngredientsItemsList.Item, ref totalGold, 15f);
            TryBuyRandomItemInto(entries, CampingCraftItemsList.Item, ref totalGold, 15f);
            TryBuyRandomItemInto(entries, CookingRecipesItemsList.Item, ref totalGold, 1f);
            TryBuyRandomItemInto(entries, CraftItemsList.Item, ref totalGold, 1f);
            TryBuyRandomItemInto(entries, CraftingSetsItemsList.Item, ref totalGold, 0.5f);
            TryBuyRandomItemInto(entries, QuiversItemsList.Item, ref totalGold, 1f);
            TryBuyRandomItemInto(entries, ScrollsItemsList.Item, ref totalGold, 5f);
            TryBuyRandomItemInto(entries, UtilityItemsList.Item, ref totalGold, 1f);
            TryBuyRandomItemInto(entries, TrashList.Item, ref totalGold, 100f);

            entries.Add(new LootEntry {
                Item  = _goldItemBlueprint.ToReference<BlueprintItemReference>(),
                Count = Mathf.Max(1, totalGold / 10)
            });
            if (entries.Count > 0) lootPart.AddItems(entries);
            
            if (UnityEngine.Random.value < 0.70f)
            {
                LootPicker.AddPickedWeaponToLoot(lootPart, cr);
            }
        }

        private static void TryBuyBestHealingPotionInto(List<LootEntry> entries, ref int totalGold, float chancePercent)
        {
            float chance = Mathf.Clamp01(chancePercent / 100f);
            if (UnityEngine.Random.value >= chance) return;

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

            var best = candidates[UnityEngine.Random.Range(0, candidates.Count)];
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
            if (UnityEngine.Random.value >= chance) return;

            var candidates = new List<(ItemData data, int cost)>();
            foreach (var it in items)
            {
                if (!int.TryParse(it.Cost, out int cost)) continue;
                if (cost <= totalGold) candidates.Add((it, cost));
            }

            if (candidates.Count == 0) return;

            var pick = candidates[UnityEngine.Random.Range(0, candidates.Count)];
            var bp = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(pick.data.AssetId);
            if (bp == null) return;

            entries.Add(new LootEntry { Item = bp.ToReference<BlueprintItemReference>(), Count = 1 });
            totalGold -= pick.cost;
        }


        internal static class EnhancementLookup
        {
            public static readonly Dictionary<BlueprintGuid, int> PlusByGuid = new Dictionary<BlueprintGuid, int>
            {
                // +1
                [BlueprintGuid.Parse("d42fc23b92c640846ac137dc26e000d4")] = 1,
                [BlueprintGuid.Parse("a9ea95c5e02f9b7468447bc1010fe152")] = 1,
                [BlueprintGuid.Parse("e90c252e08035294eba39bafce76c119")] = 1,
                // +2
                [BlueprintGuid.Parse("eb2faccc4c9487d43b3575d7e77ff3f5")] = 2,
                [BlueprintGuid.Parse("758b77a97640fd747abf149f5bf538d0")] = 2,
                [BlueprintGuid.Parse("7b9f2f78a83577d49927c78be0f7fbc1")] = 2,
                // +3
                [BlueprintGuid.Parse("80bb8a737579e35498177e1e3c75899b")] = 3,
                [BlueprintGuid.Parse("9448d3026111d6d49b31fc85e7f3745a")] = 3,
                [BlueprintGuid.Parse("ac2e3a582b5faa74aab66e0a31c935a9")] = 3,
                // +4
                [BlueprintGuid.Parse("783d7d496da6ac44f9511011fc5f1979")] = 4,
                [BlueprintGuid.Parse("eaeb89df5be2b784c96181552414ae5a")] = 4,
                [BlueprintGuid.Parse("a5d27d73859bd19469a6dde3b49750ff")] = 4,
                // +5
                [BlueprintGuid.Parse("bdba267e951851449af552aa9f9e3992")] = 5,
                [BlueprintGuid.Parse("6628f9d77fd07b54c911cd8930c0d531")] = 5,
                [BlueprintGuid.Parse("84d191a748edef84ba30c13b8ab83bd9")] = 5,
                // +6
                [BlueprintGuid.Parse("0326d02d2e24d254a9ef626cc7a3850f")] = 6,
                [BlueprintGuid.Parse("de15272d1f4eb7244aa3af47dbb754ef")] = 6,
                [BlueprintGuid.Parse("70c26c66adb96d74baec38fc8d20c139")] = 6,
            };

            public static int GetWeaponEnhancementPlus(ItemEntityWeapon w)
            {
                if (w?.Enchantments == null) return 0;

                int maxPlus = 0;
                foreach (var e in w.Enchantments)
                {
                    var bp = e?.Blueprint as BlueprintWeaponEnchantment;
                    if (bp == null) continue;

                    if (PlusByGuid.TryGetValue(bp.AssetGuid, out int plus) && plus > maxPlus)
                        maxPlus = plus;
                }
                return Mathf.Clamp(maxPlus, 0, 6);
            }
        }

        internal static class ChestScale
        {
            public static float ForPlus(int plus)
            {
                switch (Mathf.Clamp(plus, 0, 6))
                {
                    case 0: return 0.50f;
                    case 1: return 0.75f;
                    case 2: return 1.00f;
                    case 3: return 1.25f;
                    case 4: return 1.50f;
                    case 5: return 1.75f;
                    default: return 2.00f; // +6
                }
            }
        }
    }
}
