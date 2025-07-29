using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using System.Collections.Generic;
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
}
