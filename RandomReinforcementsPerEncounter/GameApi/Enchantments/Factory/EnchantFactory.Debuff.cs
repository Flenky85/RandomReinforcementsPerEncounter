using System.Collections.Generic;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;         // ApplyBuff, ContextDuration
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Utils;                              // LocalizationTool
using BlueprintCore.Utils.Types;                        // ContextValues
using Kingmaker.Blueprints;                             // BlueprintGuid
using Kingmaker.EntitySystem.Stats;                     // SavingThrowType
using Kingmaker.UnitLogic.Mechanics.Actions;            // ContextActionSavingThrow, ContextActionConditionalSaved
using Kingmaker.UnitLogic.Mechanics.Components;         // ContextSetAbilityParams
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.GameApi.Enchantments;
using RandomReinforcementsPerEncounter.Config.Localization;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Factory.Utils; // DiceMapper

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantFactory
    {
        public enum ActivationType
        {
            onlyHit,
            onlyOnFirstHit
        }

        public static void RegisterDebuffTiersFor(
            List<EnchantTierConfig> tiers,
            string name,
            string nameRoot,
            string buff,
            int durationDiceCount,
            int durationDiceSides,
            SavingThrowType savingThrowType,
            ActivationType activation,
            string description,
            string affix
        )
        {
            var buffRef = BlueprintTool.GetRef<BlueprintBuffReference>(buff);
            var diceType = DiceMapper.MapDiceType(durationDiceSides);

            bool onlyHit = activation == ActivationType.onlyHit;
            bool onlyOnFirstHit = activation == ActivationType.onlyOnFirstHit;

            for (int i = 0; i < tiers.Count; i++)
            {
     
                var EnchantTierConfig = tiers[i];
                var desc = FactoryText.BuildDebuffDescription(
                    savingThrowType,
                    EnchantTierConfig.DC,
                    description,
                    durationDiceCount,
                    durationDiceSides,
                    onlyOnFirstHit
                );
                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, name, ArtifactKind.Enchant, affix, desc);
                var nameKey = keys.nameKey;
                var descKey = keys.descKey;
                var bpName = keys.bpName;

                var locName = keys.locName;
                var locDesc = keys.locDesc;
                var locPrefix = keys.locAffix;

                // Crear el enchant vacío con nombre/desc
                WeaponEnchantmentConfigurator
                    .New(bpName, EnchantTierConfig.AssetId)
                    .SetEnchantName(locName)
                    .SetDescription(locDesc)
                    .SetPrefix(locPrefix)
                    .Configure();

                // Rama de éxito/fracaso
                var onSuccess = ActionsBuilder.New();
                var onFail = ActionsBuilder.New()
                    .ApplyBuff(
                        buffRef,
                        ContextDuration.VariableDice(diceType, durationDiceCount, isExtendable: true),
                        toCaster: false,
                        asChild: true
                    );

                var savedBranch = ActionsBuilder.New()
                    .Add<ContextActionConditionalSaved>(c =>
                    {
                        c.Succeed = onSuccess.Build();
                        c.Failed = onFail.Build();
                    });

                var onHitActions = ActionsBuilder.New()
                    .Add<ContextActionSavingThrow>(a =>
                    {
                        a.Type = savingThrowType;
                        a.FromBuff = false;
                        a.HasCustomDC = false;
                        a.Actions = savedBranch.Build();
                    });

                // Trigger + seteo de DC desde contexto
                WeaponEnchantmentConfigurator
                    .For(EnchantTierConfig.AssetId)
                    .AddInitiatorAttackWithWeaponTrigger(
                        onHitActions,
                        onlyHit: onlyHit,
                        onlyOnFirstHit: onlyOnFirstHit
                    )
                    .AddComponent<ContextSetAbilityParams>(c =>
                    {
                        c.Add10ToDC = false;
                        c.DC = ContextValues.Constant(EnchantTierConfig.DC);
                        c.CasterLevel = ContextValues.Constant(0);
                        c.Concentration = ContextValues.Constant(0);
                        c.SpellLevel = ContextValues.Constant(0);
                    })
                    .Configure();
            }
        }
    }
}
