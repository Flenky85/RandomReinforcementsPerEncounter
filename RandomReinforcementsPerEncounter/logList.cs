using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    public static class MonsterLogger
    {
        public static void LogMonsters(List<MonsterData> monsters)
        {
            foreach (var monster in monsters)
            {
                var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(monster.AssetId);
                string name = blueprint?.LocalizedName?.ToString() ?? "NombreDesconocido";

                string log = $@"
            new MonsterData
            {{
                AssetId = ""{monster.AssetId}"",  // {name}
                Levels = ""{monster.Levels}"",
                CR = ""{monster.CR}"",
                Faction = ""{monster.Faction}""
            }},";

                Debug.Log(log);
            }
        }
    }
    public static class BlueprintInspector
    {
        public static void LogBlueprintDetails(List<MonsterData> monsters)
        {
            foreach (var monster in monsters)
            {
                var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(monster.AssetId);
                if (blueprint == null)
                {
                    Debug.LogWarning($"❌ No se encontró Blueprint para AssetId {monster.AssetId}");
                    continue;
                }

                LogBlueprintSummary(blueprint);
            }
        }

        private static void LogBlueprintSummary(BlueprintUnit bp)
        {
            Debug.Log($"==== 🧠 Blueprint: {bp.name} ({bp.AssetGuid}) ====");
            Debug.Log($"Nombre: {bp.LocalizedName?.ToString() ?? "Sin nombre"}");
            Debug.Log($"CR: {bp.CR}");
            Debug.Log($"Facción: {bp.Faction?.name ?? "Ninguna"}");
            Debug.Log($"Brain: {bp.DefaultBrain?.name ?? "Sin IA"}");
            Debug.Log($"Tipo de unidad: {bp.Type?.name ?? "Ninguno"}");

            Debug.Log("🎒 Inventario:");
            if (bp.Body != null)
            {
                Debug.Log($"  - Armadura: {bp.Body.Armor?.name}");
                Debug.Log($"  - Arma Principal: {bp.Body.PrimaryHand?.name}");
                Debug.Log($"  - Arma Secundaria: {bp.Body.SecondaryHand?.name}");
            }

            Debug.Log($"🧩 Componentes ({bp.ComponentsArray?.Length ?? 0}):");
            if (bp.ComponentsArray != null)
            {
                foreach (var comp in bp.ComponentsArray)
                {
                    string compName = comp.GetType().Name;
                    string marker = EsComponentePeligroso(compName) ? "⚠️" : "✅";
                    Debug.Log($"  {marker} {compName}");
                }
            }

            Debug.Log("=============================================");
        }

        private static bool EsComponentePeligroso(string typeName)
        {
            return typeName.Contains("Quest") ||
                   typeName.Contains("Trigger") ||
                   typeName.Contains("Cutscene") ||
                   typeName.Contains("Script") ||
                   typeName.Contains("Dialog");
        }

        public static void DumpBlueprintRaw(BlueprintUnit bp)
        {
            Debug.Log($"====== 🧾 DUMP COMPLETO: {bp.name} ({bp.AssetGuid}) ======");

            var fields = bp.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                object value = field.GetValue(bp);
                if (value is BlueprintScriptableObject refBp)
                {
                    Debug.Log($" - {field.Name}: {refBp.name} ({refBp.AssetGuid})");
                }
                else if (value is IEnumerable<BlueprintScriptableObject> listRef)
                {
                    Debug.Log($" - {field.Name}: [{string.Join(", ", listRef.Select(b => b?.name ?? "null"))}]");
                }
                else if (value != null)
                {
                    Debug.Log($" - {field.Name}: {value}");
                }
                else
                {
                    Debug.Log($" - {field.Name}: null");
                }
            }

            Debug.Log("🧩 COMPONENTES:");
            if (bp.ComponentsArray != null)
            {
                foreach (var comp in bp.ComponentsArray)
                {
                    Debug.Log($" ▶️ {comp.GetType().Name}");
                    var compFields = comp.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var f in compFields)
                    {
                        object val = f.GetValue(comp);
                        string valStr = val is BlueprintScriptableObject bpo ? $"{bpo.name}" : val?.ToString() ?? "null";
                        Debug.Log($"    - {f.Name}: {valStr}");
                    }
                }
            }

            Debug.Log($"====== 🧾 FIN DUMP: {bp.name} ======");
        }
        public static void DumpBlueprintsRaw(List<MonsterData> monsters)
        {
            foreach (var monster in monsters)
            {
                var blueprint = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(monster.AssetId);
                if (blueprint == null)
                {
                    Debug.LogWarning($"❌ No se encontró Blueprint para AssetId {monster.AssetId}");
                    continue;
                }

                DumpBlueprintRaw(blueprint);
            }
        }
    }

}
