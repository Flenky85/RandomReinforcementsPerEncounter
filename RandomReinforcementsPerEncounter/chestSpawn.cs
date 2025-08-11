using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Loot;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.View.MapObjects;
using System.Collections.Generic;
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
            view.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

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
            /*
            var shortsword = ResourcesLibrary.TryGetBlueprint<BlueprintItem>("f717b39c351b8b44388c471d4d272f4e");
            if (shortsword != null)
            {
                lootEntries.Add(new LootEntry
                {
                    Item = shortsword.ToReference<BlueprintItemReference>(),
                    Count = 1
                });
            }*/
            // GUIDS
            const string ShortswordGuid = "f717b39c351b8b44388c471d4d272f4e"; // Shortsword simple
            const string EnchantPlus1Guid = "d42fc23b92c640846ac137dc26e000d4"; // Enhancement +1
            //const string EnchantCorrosiveGuid = "633b38ff1d11de64a91d490c683ab1c8"; // Corrosive 1d6
            

            var shortswordBp = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(ShortswordGuid);
            var enchBp = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(EnchantPlus1Guid);
            //var enchCorro = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(EnchantCorrosiveGuid);
            /*var enchCorro1d8 = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(
                GuidUtil.FromString("corrosive.1d8")
            );*/
            var clone = EnchantMaker.CloneCorrosive1d8();
            var cache = (BlueprintsCache)AccessTools
                .Property(typeof(BlueprintsCache), "Instance")
                .GetValue(null);
            cache.AddCachedBlueprint(clone.AssetGuid, clone);
            Debug.Log("[RRE] Clonado corrosive 1d6 -> corrosive.1d8: " + clone.AssetGuid);

            if (shortswordBp != null && enchBp != null)
            {

                var item = shortswordBp.CreateEntity();
                if (item is ItemEntityWeapon weap)
                {
                    weap.AddEnchantment(enchBp, null); // permanente en esta instancia
                    //weap.AddEnchantment(clone, null); // permanente en esta instancia
                    
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

            lootPart.AddItems(lootEntries);
        }
    }
}

