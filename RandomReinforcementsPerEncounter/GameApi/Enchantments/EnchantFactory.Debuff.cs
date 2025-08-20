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
using RandomReinforcementsPerEncounter.Domain.Text;
using RandomReinforcementsPerEncounter.GameApi.Localization;
using RandomReinforcementsPerEncounter.GameApi.Enchantments; // DiceMapper

namespace RandomReinforcementsPerEncounter
{
    public static partial class EnchantFactory
    {
        public enum ActivationType
        {
            onlyHit,
            onlyOnFirstHit
        }

        public static void RegisterDebuffTiersFor(
            List<TierConfig> tiers,
            string name,
            string nameRoot,
            string buff,
            int durationDiceCount,
            int durationDiceSides,
            SavingThrowType savingThrowType,
            ActivationType activation,
            string description
        )
        {
            var buffRef = BlueprintTool.GetRef<BlueprintBuffReference>(buff);
            var diceType = DiceMapper.MapDiceType(durationDiceSides);

            bool onlyHit = activation == ActivationType.onlyHit;
            bool onlyOnFirstHit = activation == ActivationType.onlyOnFirstHit;

            for (int i = 0; i < tiers.Count; i++)
            {
                var tierConfig = tiers[i];
                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, name, ArtifactKind.Enchant);
                var nameKey = keys.nameKey;
                var descKey = keys.descKey;
                var bpName = keys.bpName;

                var locName = keys.locName;
                var locDesc = LocalizationTool.CreateString(
                    descKey,
                    BuildDescription(
                        savingThrowType,
                        tierConfig.DC,
                        description,
                        durationDiceCount,
                        durationDiceSides,
                        onlyOnFirstHit
                    )
                );
                var locPrefix = keys.locPrefix;

                // Crear el enchant vacío con nombre/desc
                WeaponEnchantmentConfigurator
                    .New(bpName, tierConfig.AssetId)
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
                    .For(tierConfig.AssetId)
                    .AddInitiatorAttackWithWeaponTrigger(
                        onHitActions,
                        onlyHit: onlyHit,
                        onlyOnFirstHit: onlyOnFirstHit
                    )
                    .AddComponent<ContextSetAbilityParams>(c =>
                    {
                        c.Add10ToDC = false;
                        c.DC = ContextValues.Constant(tierConfig.DC);
                        c.CasterLevel = ContextValues.Constant(0);
                        c.Concentration = ContextValues.Constant(0);
                        c.SpellLevel = ContextValues.Constant(0);
                    })
                    .Configure();
            }
        }

        private static string BuildDescription(
            SavingThrowType saveType,
            int dc,
            string conditionText,
            int durationCount,
            int durationSides,
            bool onlyOnFirstHit
        )
        {
            string intro = onlyOnFirstHit
                ? "The first time this weapon hits a given enemy, that enemy must pass a "
                : "Whenever this weapon lands a hit, the enemy must pass a ";

            string saveName = saveType.ToString().ToLowerInvariant();
            string diceText = durationSides == 1 ? durationCount.ToString() : $"{durationCount}d{durationSides}";
            bool isExactlyOneRound = durationSides == 1 && durationCount == 1;
            string roundText = isExactlyOneRound ? "round" : "rounds";

            string plain = $"{intro}{saveName} saving throw (DC {dc}) or become {conditionText} for {diceText} {roundText}.";
            return AutoLinker.Apply(plain);
        }
    }
}
