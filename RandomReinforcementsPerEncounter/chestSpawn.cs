using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Items;
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

            var longsword = ResourcesLibrary.TryGetBlueprint<BlueprintItem>("03d706655c07d804cb9d5a5583f9aec5");
            if (longsword != null)
            {
                lootEntries.Add(new LootEntry
                {
                    Item = longsword.ToReference<BlueprintItemReference>(),
                    Count = 1
                });
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

