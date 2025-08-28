using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Area;
using Kingmaker.EntitySystem.Persistence;

namespace RandomReinforcementsPerEncounter.Patches
{
    [HarmonyPatch(typeof(Game), nameof(Game.LoadGame))]
    internal static class Game_LoadGame_ResetCombatFlags
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            State.CombatFlags.Reset();
            State.LootContext.Reset();
        }
    }

    // overload sin parámetros
    [HarmonyPatch(typeof(Game), nameof(Game.LoadNewGame), new System.Type[] { })]
    internal static class Game_LoadNewGame_NoArgs_ResetCombatFlags
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            State.CombatFlags.Reset();
            State.LootContext.Reset();
        }
    }

    // overload con (BlueprintAreaPreset, SaveInfo)
    [HarmonyPatch(typeof(Game), nameof(Game.LoadNewGame), new System.Type[] { typeof(BlueprintAreaPreset), typeof(SaveInfo) })]
    internal static class Game_LoadNewGame_ResetCombatFlags
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            State.CombatFlags.Reset();
            State.LootContext.Reset();
        }
    }
}
