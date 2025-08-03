using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Loot;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.View.MapObjects;
using System;
using System.Reflection;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;


namespace RandomReinforcementsPerEncounter
{
    
    public static class ChestSpawn
    {
        public static Vector3? storedChestPosition = null;

        public static void SpawnLootChest(string blueprintGuid, Vector3 position)
        {
            Debug.Log("[LootChest] 📦 Spawn started...");

            var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintDynamicMapObject>(blueprintGuid);
            if (blueprint == null || blueprint.Prefab == null)
            {
                Debug.LogError("[LootChest] ❌ Invalid blueprint or prefab.");
                return;
            }

            var sceneEntities = Game.Instance.State.LoadedAreaState.MainState;
            var rotation = Quaternion.identity;
            var mapEntityData = Game.Instance.EntityCreator.SpawnMapObject(blueprint, position, rotation, sceneEntities);
            if (mapEntityData?.View == null)
            {
                Debug.LogError("[LootChest] ❌ View is null after spawn.");
                return;
            }
            var data = mapEntityData?.View?.Data as MapObjectEntityData;
            if (data != null)
            {
                var lootPart1 = data.Parts.Get<InteractionLootPart>();
                if (lootPart1 == null)
                {
                    Debug.Log("[LootChest] 🧩 InteractionLootPart missing — adding via generic Add<T>()");
                    lootPart1 = data.Parts.Add<InteractionLootPart>();
                }
                else
                {
                    Debug.Log("[LootChest] ✅ InteractionLootPart already present.");
                }
            }
            else
            {
                Debug.LogWarning("[LootChest] ⚠️ No valid MapObjectEntityData found.");
            }




            var go = mapEntityData.View.gameObject;
            var interactionLoot = go.GetComponent<InteractionLoot>() ?? go.AddComponent<InteractionLoot>();

            if (interactionLoot.Settings == null)
            {
                Debug.LogWarning("[LootChest] ⚠️ Settings is null!");
            }
            else
            {
                Debug.Log("[LootChest] ✅ Settings exists.");
            }
            var test = ResourcesLibrary.TryGetBlueprint<BlueprintScriptableObject>("62a9f5357c9e58f4abebb4aa4f291842");
            Debug.Log($"[LootChest] Test blueprint: {test?.name ?? "null"}");

            Debug.Log("[LootChest] 🔍 Attempting to load loot blueprint...");
            var myLootBlueprint = ResourcesLibrary.TryGetBlueprint<BlueprintLoot>("62a9f5357c9e58f4abebb4aa4f291842");
            Debug.Log("[LootChest] 📦 Loot blueprint loaded.");

            if (myLootBlueprint == null)
            {
                Debug.LogError("[LootChest] ❌ Failed to load loot blueprint.");
                return;
            }

            var field = typeof(InteractionLootSettings).GetField("m_LootTables", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
            {
                Debug.LogError("[LootChest] ❌ Could not get m_LootTables field via reflection.");
                return;
            }

            if (myLootBlueprint == null)
            {
                Debug.LogError("[LootChest] ❌ Loot blueprint is null.");
                return;
            }

            Debug.Log("[LootChest] 🔧 Setting loot table via reflection...");
            try
            {
                field.SetValue(interactionLoot.Settings, new BlueprintLootReference[]
                {
                    myLootBlueprint.ToReference<BlueprintLootReference>()
                });
                Debug.Log("[LootChest] ✅ Loot table assigned.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LootChest] ❌ Exception during SetValue: {ex.Message}");
                return;
            }

            var lootPart = data?.Parts?.Get<InteractionLootPart>();
            if (lootPart == null)
            {
                Debug.LogError("[LootChest] ❌ No InteractionLootPart found in Parts.");
            }
            else
            {
                Debug.Log("[LootChest] ➕ Attempting to add MapObjectLoot...");
                lootPart.AddItems(lootPart.MapObjectLoot);
            }


            Debug.Log("[LootChest] ✅ SpawnLootChest completed.");
        }


    }
}