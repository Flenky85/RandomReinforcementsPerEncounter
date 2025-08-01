using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.View.MapObjects;
using TurnBased.Controllers;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    [HarmonyPatch(typeof(CombatController), "HandleCombatEnd")]
    public static class CombatEndPatch
    {
        //var level = unit.Descriptor.Progression.m_CharacterLevel; // Esto solo es posible si está publicized
        static void Postfix()
        {
            Debug.Log("[ChestSpawn] 📦 Entrando en CombatEndPatch.Postfix");

            if (ChestSpawn.StoredPosition != null)
            {
                Debug.Log($"[ChestSpawn] 📌 Posición almacenada detectada: {ChestSpawn.StoredPosition}");

                try
                {
                    // Usamos un BlueprintDynamicMapObject en vez de BlueprintSpawnableObject
                    string assetId = "a3f0ed9a361b4e5eb84e1ab8abd77a67"; // DLC3_LootContainer_2
                    Debug.Log($"[ChestSpawn] 🔍 Intentando cargar BlueprintDynamicMapObject con GUID {assetId}");

                    var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintDynamicMapObject>(assetId);

                    if (blueprint == null)
                    {
                        Debug.LogError("[ChestSpawn] ❌ Blueprint es null.");
                    }
                    else if (blueprint.Prefab == null)
                    {
                        Debug.LogError("[ChestSpawn] ❌ Prefab del blueprint es null.");
                    }
                    else
                    {
                        Debug.Log("[ChestSpawn] 🛠️ Prefab localizado, intentando instanciar...");

                        var prefab = blueprint.Prefab; // NOT .Load() porque es GameObject directo
                        var go = GameObject.Instantiate(prefab);
                        go.transform.position = ChestSpawn.StoredPosition.Value;

                        var view = go.GetComponent<MapObjectView>();
                        if (view != null)
                        {
                            view.gameObject.SetActive(true); // por si acaso
                            view.enabled = true;             // asegura que el componente se active
                            Debug.Log("[ChestSpawn] ✅ Cofre instanciado y activado.");
                        }
                        else
                        {
                            Debug.LogWarning("[ChestSpawn] ⚠️ Objeto instanciado sin componente MapObjectView.");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[ChestSpawn] 💥 Excepción al intentar spawnear el cofre: {ex}");
                }
            }

            ChestSpawn.StoredPosition = null;
            Debug.Log("[ChestSpawn] 🔄 Posición limpiada al final del combate.");
        }
    }
}




