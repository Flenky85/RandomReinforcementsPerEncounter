using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Loot;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.View.MapObjects;
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

            var gameObject = view.gameObject;
            var interactionLoot = gameObject.GetComponent<InteractionLoot>() ?? gameObject.AddComponent<InteractionLoot>();

            var lootBlueprint = ResourcesLibrary.TryGetBlueprint<BlueprintLoot>("62a9f5357c9e58f4abebb4aa4f291842");
            if (lootBlueprint == null) return;

            var lootField = typeof(InteractionLootSettings).GetField("m_LootTables", BindingFlags.NonPublic | BindingFlags.Instance);
            if (lootField == null || interactionLoot.Settings == null) return;

            try
            {
                lootField.SetValue(interactionLoot.Settings, new[]
                {
                    lootBlueprint.ToReference<BlueprintLootReference>()
                });
            }
            catch
            {
                return;
            }

            lootPart.AddItems(lootPart.MapObjectLoot);
        }
    }
}

