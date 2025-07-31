using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Cambiar el assetid
//quitar exp
//quitar loot.
//elimnar facts CombatStateTrigger

namespace RandomReinforcementsPerEncounter
{
    public static class ManualBlueprintCloner
    {
        public static BlueprintUnit CloneMinimal(BlueprintUnit original)
        {
            Debug.Log($"[Cloner] 🔁 Iniciando clonado de: {original?.name} ({original?.AssetGuid})");
            if (original == null)
            {
                Debug.LogError("❌ Blueprint original es null.");
                return null;
            }
            Debug.Log("[Cloner] Paso 1 completado: ID y nombre");
            /*
            var clone = new BlueprintUnit
            {
                name = original.name + "_Clone",
                AssetGuid = new BlueprintGuid(Guid.NewGuid()),
                LocalizedName = original.LocalizedName,
                Gender = original.Gender,
                Size = original.Size,
                Alignment = original.Alignment,
                Faction = original.Faction,
                Visual = original.Visual,
                Prefab = original.Prefab,
                Body = original.Body,
                Speed = original.Speed,
                Strength = original.Strength,
                Dexterity = original.Dexterity,
                Constitution = original.Constitution,
                Intelligence = original.Intelligence,
                Wisdom = original.Wisdom,
                Charisma = original.Charisma,
                Skills = original.Skills,
                MaxHP = original.MaxHP,
                BaseAttackBonus = original.BaseAttackBonus
            };*/
            var clone = new BlueprintUnit
            {
                name = original.name + "_Clone",
                AssetGuid = new BlueprintGuid(Guid.NewGuid())
            };
            clone.LocalizedName = original.LocalizedName;
            clone.Gender = original.Gender;
            clone.Size = original.Size;
            clone.Alignment = original.Alignment;
            clone.FactionOverrides = original.FactionOverrides;
            clone.AlternativeBrains = original.AlternativeBrains;
            clone.Faction = original.Faction;
            clone.Visual = original.Visual;
            clone.Prefab = original.Prefab;
            clone.Body = original.Body;
            clone.Speed = original.Speed;
            clone.Strength = original.Strength;
            clone.Dexterity = original.Dexterity;
            clone.Constitution = original.Constitution;
            clone.Intelligence = original.Intelligence;
            clone.Wisdom = original.Wisdom;
            clone.Charisma = original.Charisma;
            clone.Skills = original.Skills;
            clone.MaxHP = original.MaxHP;
            clone.BaseAttackBonus = original.BaseAttackBonus;

            var t = typeof(BlueprintUnit);
            var typeField = t.GetField("m_Type", BindingFlags.NonPublic | BindingFlags.Instance);
            typeField?.SetValue(clone, typeField.GetValue(original));
            var raceField = t.GetField("m_Race", BindingFlags.NonPublic | BindingFlags.Instance);
            raceField?.SetValue(clone, raceField.GetValue(original));
            var portraitField = t.GetField("m_Portrait", BindingFlags.NonPublic | BindingFlags.Instance);
            portraitField?.SetValue(clone, portraitField.GetValue(original));
            var brainField = t.GetField("m_Brain", BindingFlags.NonPublic | BindingFlags.Instance);
            brainField?.SetValue(clone, brainField.GetValue(original));
            var templatesField = t.GetField("m_AdditionalTemplates", BindingFlags.NonPublic | BindingFlags.Instance);
            templatesField?.SetValue(clone, templatesField.GetValue(original));
            var elementsField = typeof(SimpleBlueprint).GetField("m_AllElements", BindingFlags.NonPublic | BindingFlags.Instance);
            elementsField?.SetValue(clone, elementsField.GetValue(original));

            var invField = typeof(BlueprintUnit).GetField("m_StartingInventory", BindingFlags.NonPublic | BindingFlags.Instance);
            var inventoryReferences = original.StartingInventory
                .Select(item => item.ToReference<BlueprintItemReference>())
                .ToArray();
            invField?.SetValue(clone, inventoryReferences);
     
            var factsField = typeof(BlueprintUnit).GetField("m_AddFacts", BindingFlags.NonPublic | BindingFlags.Instance);
            var factsReferences = original.AddFacts
                .Select(fact => fact.ToReference<BlueprintUnitFactReference>())
                .ToArray();
            factsField?.SetValue(clone, factsReferences);

            // Copiar componentes, filtrando los peligrosos
            clone.ComponentsArray = original.ComponentsArray?
                .Where(c => c != null && !c.GetType().Name.Contains("CombatStateTrigger"))
                .ToArray() ?? Array.Empty<BlueprintComponent>();

            // Quitar experiencia: eliminar componentes de tipo AddUnitExperience
            var experienceComponent = clone.ComponentsArray
                .FirstOrDefault(c => c?.GetType().Name == "Experience");


            var addLootComponent = clone.ComponentsArray
                .FirstOrDefault(c => c?.GetType().Name == "AddLoot");

            if (addLootComponent != null)
            {
                var disabledField = addLootComponent.GetType().GetField("Disabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (disabledField != null)
                {
                    disabledField.SetValue(addLootComponent, false);
                    Debug.Log("[Cloner] 🔕 Componente 'AddLoot' desactivado (Disabled = true)");
                }
                else
                {
                    Debug.LogWarning("[Cloner] ⚠️ No se encontró el campo 'Disabled' en AddLoot");
                }
            }

            // Quitar loot: establecer m_Loot a null
            var lootField = typeof(BlueprintUnit).GetField("m_Loot", BindingFlags.NonPublic | BindingFlags.Instance);
            lootField?.SetValue(clone, null);

            ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(clone.AssetGuid, clone);

            return clone;
        }
    }
}
