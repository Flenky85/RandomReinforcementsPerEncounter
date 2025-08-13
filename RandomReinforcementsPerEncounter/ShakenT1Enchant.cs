using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;    // ApplyBuff, SavingThrow, ContextDuration
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Utils;  // LocalizationTool, LocalString
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints; // BlueprintGuid
using Kingmaker.EntitySystem.Stats;           // SavingThrowType
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;   // BlueprintBuffReference


namespace RandomReinforcementsPerEncounter
{
    public static class ShakenT1Enchant
    {
        public static readonly BlueprintGuid AssetGuid = GuidUtil.FromString("shaken.t1");
        private const string NameKey = "RRE.ShakenT1.Name";
        private const string DescKey = "RRE.ShakenT1.Description";

        public static void Register()
        {
            var name = LocalizationTool.CreateString(NameKey, "Shaking Edge (T1)");
            var desc = LocalizationTool.CreateString(DescKey, "This weapon hums with unsettling power.");

            // Asegúrate de crear el enchant (si ya existía, For(...) abajo lo cogerá)
            WeaponEnchantmentConfigurator
                .New("RRE_ShakenT1_Enchant", AssetGuid.ToString())
                .SetEnchantName(name)
                .SetDescription(desc)
                .Configure();

            // Buff shaken del juego
            var shakenBuffRef = BlueprintTool.GetRef<BlueprintBuffReference>("25ec6cb6ab1845c48a95f9c20b034220");

            // onSuccess: no hace nada
            var onSuccess = ActionsBuilder.New();

            // onFail: ApplyBuff 1d3 asaltos, extendible
            var onFail = ActionsBuilder.New()
                .ApplyBuff(
                    shakenBuffRef,
                    ContextDuration.VariableDice(DiceType.D3, 1, isExtendable: true),
                    toCaster: false,
                    asChild: true
                );

            // 1) Construimos el ConditionalSaved **a mano** con los ActionList ya Build()
            var savedBranch = ActionsBuilder.New()
                .Add<ContextActionConditionalSaved>(c =>
                {
                    c.Succeed = onSuccess.Build();
                    c.Failed = onFail.Build();
                });

            // 2) Ahora el SavingThrow que envuelve ese ConditionalSaved
            var onHitActions = ActionsBuilder.New()
                .Add<ContextActionSavingThrow>(a =>
                {
                    a.Type = SavingThrowType.Fortitude;
                    a.FromBuff = false;
                    a.HasCustomDC = false; // ya meteremos DC fija si quieres
                    a.Actions = savedBranch.Build();
                });

            // Trigger al impactar: usa SOLO parámetros que tu firma soporta
            WeaponEnchantmentConfigurator
                .For(AssetGuid.ToString())
                .AddInitiatorAttackWithWeaponTrigger(
                    onHitActions,                 // <-- primer parámetro posicional: ActionsBuilder
                    onlyHit: true                 // <-- equivalente a tu JSON
                                                  // El resto lo dejamos por defecto/omitido (coincide con tus valores en false/null)
                )
                // --- DC fija 23 para el saving throw de este contexto ---
                .AddComponent<ContextSetAbilityParams>(c =>
                {
                    c.Add10ToDC = false;
                    c.DC = ContextValues.Constant(23);
                    c.CasterLevel = ContextValues.Constant(0);
                    c.Concentration = ContextValues.Constant(0);
                    c.SpellLevel = ContextValues.Constant(0);
                })
                .Configure();
        }
    }
}
