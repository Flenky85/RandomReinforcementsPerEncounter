using UnityModManagerNet;
using HarmonyLib;

namespace RandomReinforcementsPerEncounter
{
    public static class Main
    {
        public static bool Enabled;

        public static void Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;

            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();

            modEntry.Logger.Log("RandomReinforcementsPerEncounter Load with Harmony");
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }
    }
}