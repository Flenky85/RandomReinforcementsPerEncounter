/*using Kingmaker.Blueprints;
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

namespace RandomReinforcementsPerEncounter
{
    public static class ManualBlueprintCloner
    {
        public static BlueprintUnit CloneMinimal(BlueprintUnit original)
        {

            if (original == null)
            {
                return null;
            }

            var clone = new BlueprintUnit
            {
                name = original.name + "_Clone",
                AssetGuid = new BlueprintGuid(Guid.NewGuid()),

                LocalizedName = original.LocalizedName,
                Gender = original.Gender,
                Size = original.Size,
                Alignment = original.Alignment,
                FactionOverrides = original.FactionOverrides,
                AlternativeBrains = original.AlternativeBrains,
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
                BaseAttackBonus = original.BaseAttackBonus,

                m_Type = original.m_Type,
                m_Race = original.m_Race,
                m_Brain = original.m_Brain,
                m_Portrait = original.m_Portrait,
                m_AdditionalTemplates = original.m_AdditionalTemplates,
                m_AllElements = original.m_AllElements,

                m_StartingInventory = original.m_StartingInventory.ToArray(),
                m_AddFacts = original.m_AddFacts.ToArray(),

            };

            // Copiar componentes sin CombatStateTrigger
            var safeComponents = original.ComponentsArray?
                .Where(c => c != null && !c.GetType().Name.Contains("CombatStateTrigger"))
                .ToArray() ?? Array.Empty<BlueprintComponent>();

            // Desactivar loot si lo hay
            foreach (var comp in safeComponents)
            {
                if (comp.GetType().Name == "AddLoot")
                {
                    var prop = comp.GetType().GetProperty("Disabled");
                    if (prop != null)
                    {
                        prop.SetValue(comp, true);
                    }
                }
            }

            clone.ComponentsArray = safeComponents;

            ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(clone.AssetGuid, clone);

            return clone;
        }
    }
}*/
