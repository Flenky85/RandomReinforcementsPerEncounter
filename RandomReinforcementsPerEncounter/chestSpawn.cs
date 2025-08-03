using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Loot;
using Kingmaker.View.MapObjects;
using System.Reflection;
using UnityEngine;


namespace RandomReinforcementsPerEncounter
{
    
    public static class ChestSpawn
    {
        public static Vector3? storedChestPosition = null;

        public static void SpawnLootChest(string blueprintGuid, Vector3 position)
        {
            var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintDynamicMapObject>(blueprintGuid);
            if (blueprint == null || blueprint.Prefab == null)
                return;

            var sceneEntities = Game.Instance.State.LoadedAreaState.MainState;
            var rotation = Quaternion.identity;
            var mapEntityData = Game.Instance.EntityCreator.SpawnMapObject(blueprint, position, rotation, sceneEntities);
            if (mapEntityData?.View == null)
                return;

            var go = mapEntityData.View.gameObject;
            var interactionLoot = go.GetComponent<InteractionLoot>() ?? go.AddComponent<InteractionLoot>();
            var myLootBlueprint = ResourcesLibrary.TryGetBlueprint<BlueprintLoot>("4b61f0c1f3644c55a0e639b6d884bb7f");
            var field = typeof(InteractionLootSettings).GetField("m_LootTables", BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(interactionLoot.Settings, new BlueprintLootReference[]
            {
                myLootBlueprint.ToReference<BlueprintLootReference>()
            });

            var lootPart = go.GetComponent<InteractionLootPart>();
            lootPart?.AddItems(lootPart.MapObjectLoot);
        }

    }
}