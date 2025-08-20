using HarmonyLib;
using Kingmaker.EntitySystem.Persistence;
using RandomReinforcementsPerEncounter.GameApi.Clones;

namespace RandomReinforcementsPerEncounter.Patches.Save
{
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.SaveRoutine))]
    internal static class SaveManagerSaveRoutinePatch
    {
        [HarmonyPrefix]
        private static void Prefix(SaveInfo saveInfo)
        {
            CloneUtility.DestroyAllClones();
        }
    }
}
