using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Loot;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence.JsonUtility;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.View.MapObjects;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    public static class ChestSpawn
    {
        public static void SpawnLootChest(string chestBlueprintGuid, Vector3 position)
        {
            var chestBlueprint = ResourcesLibrary.TryGetBlueprint<BlueprintDynamicMapObject>(chestBlueprintGuid);
            if (chestBlueprint?.Prefab == null) return;

            var mapEntityData = Game.Instance.EntityCreator.SpawnMapObject(
                chestBlueprint,
                position,
                Quaternion.identity,
                Game.Instance.State.LoadedAreaState.MainState
            );

            var view = mapEntityData?.View;
            if (view == null) return;

            var entityData = view.Data as MapObjectEntityData;
            if (entityData == null) return;

            var lootPart = entityData.Parts.Get<InteractionLootPart>() ?? entityData.Parts.Add<InteractionLootPart>();

            var destroyField = typeof(InteractionLootPart).GetField("m_DestroyWhenEmpty", BindingFlags.Instance | BindingFlags.NonPublic);
            destroyField?.SetValue(lootPart, true);

            lootPart.AlreadyUnlocked = true;

            var gameObject = view.gameObject;
            var interactionLoot = gameObject.GetComponent<InteractionLoot>() ?? gameObject.AddComponent<InteractionLoot>();

            var dummyLootBlueprint = ResourcesLibrary.TryGetBlueprint<BlueprintLoot>("931f5cd963df3984ba96562ae0b206dd");
            if (dummyLootBlueprint == null) return;

            var lootField = typeof(InteractionLootSettings).GetField("m_LootTables", BindingFlags.NonPublic | BindingFlags.Instance);
            if (lootField == null || interactionLoot.Settings == null) return;

            lootField.SetValue(interactionLoot.Settings, new[]
            {
                dummyLootBlueprint.ToReference<BlueprintLootReference>()
            });

            var lootEntries = new List<LootEntry>();

            foreach (var cr in LootContext.EnemyCRs)
            {
                AddLootFromEnemyCRs(lootPart, cr);
            }

            /*
            if (view != null)
            {
                view.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }*/
            int maxPlus =
            lootPart?.Loot?.Items?
                .OfType<ItemEntityWeapon>()
                .Select(EnhancementLookup.GetWeaponEnhancementPlus)
                .DefaultIfEmpty(0)
                .Max() ?? 0;

            float s = ChestScale.ForPlus(maxPlus);
            view.transform.localScale = new Vector3(s, s, s);



            /*
            const string ShortswordGuid = "f717b39c351b8b44388c471d4d272f4e"; // Shortsword simple
            const string EnchantPlus1Guid = "d42fc23b92c640846ac137dc26e000d4"; // Enhancement +1
                       
            var shortswordBp = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(ShortswordGuid);
            var enchBp = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(EnchantPlus1Guid);
            var enchanttest = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(
                GuidUtil.EnchantGuid("corrosive.t2")
            );

            var ctx = new MechanicsContext(default(JsonConstructorMark));



            if (shortswordBp != null && enchBp != null)
            {

                var item = shortswordBp.CreateEntity();
                if (item is ItemEntityWeapon weap)
                {
                    weap.AddEnchantment(enchBp, ctx); // permanente en esta instancia
                    weap.AddEnchantment(enchanttest, ctx); // permanente en esta instancia
                    
                    lootPart.Loot.Add(weap);           // usar directamente la ItemsCollection existente
                }
            }


            var gold = ResourcesLibrary.TryGetBlueprint<BlueprintItem>("f2bc0997c24e573448c6c91d2be88afa");
            if (gold != null)
            {
                lootEntries.Add(new LootEntry
                {
                    Item = gold.ToReference<BlueprintItemReference>(),
                    Count = 3
                });
            }

            lootPart.AddItems(lootEntries);*/
        }
        private static readonly Dictionary<int, int> GoldByCR = new Dictionary<int, int>
        {
            { 1, 10 }, { 2, 20 }, { 3, 30 }, { 4, 50 }, { 5, 70 },
            { 6, 100 }, { 7, 130 }, { 8, 160 }, { 9, 200 }, { 10, 240 },
            { 11, 280 }, { 12, 330 }, { 13, 380 }, { 14, 440 }, { 15, 500 },
            { 16, 600 }, { 17, 700 }, { 18, 800 }, { 19, 900 }, { 20, 1000 },
            { 21, 1100 }, { 22, 1200 }, { 23, 1300 }, { 24, 1400 }, { 25, 1500 },
            { 26, 1600 }, { 27, 1700 }, { 28, 1800 }, { 29, 1900 }, { 30, 2000 }
        };
        private static void AddLootFromEnemyCRs(InteractionLootPart lootPart, int cr)
        {
            const string GoldBlueprintId = "f2bc0997c24e573448c6c91d2be88afa";
            var gold = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(GoldBlueprintId);

            int totalGold = 0;

            int baseGold = GoldByCR.TryGetValue(cr, out int v) ? v : 2000;

            // Sesgo cuadrático estilo Excel: 1 + 9 * (rand^2)
            float r = UnityEngine.Random.value;
            float bias = 1f + 9f * Mathf.Pow(r, 2f);
            int finalGold = Mathf.RoundToInt(baseGold * bias);

            totalGold += Mathf.Max(1, finalGold);
            
            TryBuyRandomItem(PermanetBuffsItemsList.Item, lootPart, ref totalGold, 0.1f);
            TryBuyBestHealingPotion(lootPart, ref totalGold, 10f);
            TryBuyRandomItem(PotionsItemsList.Item, lootPart, ref totalGold, 5f);
            TryBuyRandomItem(ConsumiblesList.Item, lootPart, ref totalGold, 2.5f);
            TryBuyRandomItem(PurifyingSolutionItemsList.Item, lootPart, ref totalGold, 5f);
            TryBuyRandomItem(CookingIngredientsItemsList.Item, lootPart, ref totalGold, 15f);
            TryBuyRandomItem(CampingCraftItemsList.Item, lootPart, ref totalGold, 15f);
            TryBuyRandomItem(CookingRecipesItemsList.Item, lootPart, ref totalGold, 1f);
            TryBuyRandomItem(CraftItemsList.Item, lootPart, ref totalGold, 1f);
            TryBuyRandomItem(CraftingSetsItemsList.Item, lootPart, ref totalGold, 0.5f);
            TryBuyRandomItem(QuiversItemsList.Item, lootPart, ref totalGold, 1f);
            TryBuyRandomItem(ScrollsItemsList.Item, lootPart, ref totalGold, 5f);
            TryBuyRandomItem(UtilityItemsList.Item, lootPart, ref totalGold, 1f);
            TryBuyRandomItem(TrashList.Item, lootPart, ref totalGold, 100f);

            lootPart.AddItems(new List<LootEntry>
            {
                new LootEntry
                {
                    Item = gold.ToReference<BlueprintItemReference>(),
                    Count = Mathf.Max(1, totalGold / 10)
                }
            });
            if (Random.value < 0.70f)
            {
                LootPicker.AddPickedWeaponToLoot(lootPart, cr);
            }
        }

        /// <summary>
        /// Con un 10% de probabilidad, compra la poción más cara posible de PotionshealsItemsList
        /// cuyo coste sea ≤ totalGold, la añade al cofre y descuenta su coste de totalGold.
        /// </summary>
        private static void TryBuyBestHealingPotion(InteractionLootPart lootPart, ref int totalGold, float chancePercent)
        {
            float chance = Mathf.Clamp01(chancePercent / 100f);
            if (UnityEngine.Random.value >= chance) return;

            var candidates = new List<ItemData>();
            int maxCost = 0;

            // Reunir todas las asequibles con coste máximo
            foreach (var it in PotionshealsItemsList.Item)
            {
                if (!int.TryParse(it.Cost, out int cost)) continue;
                if (cost > totalGold) continue;

                if (cost > maxCost)
                {
                    maxCost = cost;
                    candidates.Clear();
                    candidates.Add(it);
                }
                else if (cost == maxCost)
                {
                    candidates.Add(it);
                }
            }

            if (candidates.Count == 0) return;

            // Elegir aleatoria entre las de coste máximo
            var best = candidates[UnityEngine.Random.Range(0, candidates.Count)];
            var potionBp = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(best.AssetId);
            if (potionBp == null) return;

            lootPart.AddItems(new List<LootEntry>
            {
                new LootEntry
                {
                    Item = potionBp.ToReference<BlueprintItemReference>(),
                    Count = 1
                }
            });

            totalGold -= maxCost;
        }

        // Wrapper por defecto (usa tu lista global)
        private static void TryBuyRandomItem(InteractionLootPart lootPart, ref int totalGold, float chancePercent)
            => TryBuyRandomItem(PotionsItemsList.Item, lootPart, ref totalGold, chancePercent);

        // Sobrecarga que acepta la lista a usar
        private static void TryBuyRandomItem(IEnumerable<ItemData> items,
                                               InteractionLootPart lootPart,
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

            lootPart.AddItems(new List<LootEntry>
            {
                new LootEntry { Item = bp.ToReference<BlueprintItemReference>(), Count = 1 }
            });

            totalGold -= pick.cost;
        }
        internal static class EnhancementLookup
        {
            // Mapa GUID -> +X (incluye arma/armadura/escudo; filtramos por tipo al leer)
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

            // Devuelve el +X estrictamente de ARMA (ignora enchants de armadura/escudo)
            public static int GetWeaponEnhancementPlus(ItemEntityWeapon w)
            {
                if (w?.Enchantments == null) return 0;

                int maxPlus = 0;
                foreach (var e in w.Enchantments)
                {
                    // Solo contamos si el blueprint es de arma
                    var bp = e?.Blueprint as BlueprintWeaponEnchantment;
                    if (bp == null) continue;

                    if (PlusByGuid.TryGetValue(bp.AssetGuid, out int plus))
                        if (plus > maxPlus) maxPlus = plus;
                }
                return Mathf.Clamp(maxPlus, 0, 6);
            }
        }

        internal static class ChestScale
        {
            // Mapea +X a la escala fija que pediste
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

