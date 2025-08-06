using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using RandomReinforcementsPerEncounter;
using System.Collections.Generic;
using UnityEngine;
using Kingmaker.Items;

//Se llama de esta forma en la consola unityexplorer
//ItemTestUtils.SpawnItemListToInventory(RandomReinforcementsPerEncounter.ShirtTypeItemsList.Item);

public static class ItemTestUtils
{
    public static void SpawnItemListToInventory(List<ItemData> itemList)
    {
        if (itemList == null || itemList.Count == 0)
        {
            Debug.Log("⚠️ Item list is empty or null.");
            return;
        }

        int count = 0;

        foreach (var entry in itemList)
        {
            if (string.IsNullOrWhiteSpace(entry.AssetId))
                continue;

            var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(entry.AssetId);
            if (blueprint == null)
            {
                Debug.LogWarning($"❌ Item not found: {entry.AssetId} ({entry.Name})");
                continue;
            }

            var item = blueprint.CreateEntity();
            item.SetCount(1);

            Game.Instance.Player.Inventory.Add(item);
            Debug.Log($"✅ Added: {entry.Name} ({entry.AssetId})");
            count++;
        }

        Debug.Log($"🎒 Total items added to inventory: {count}");
    }
}

