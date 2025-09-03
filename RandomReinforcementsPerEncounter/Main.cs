using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using RandomReinforcementsPerEncounter.GameApi.Clones;
using RandomReinforcementsPerEncounter.UI;

namespace RandomReinforcementsPerEncounter
{
    public static class Main
    {
        public static bool Enabled;

        public static UnityModManager.ModEntry ModEntry;
        public static Harmony Harmony;
        public static bool FatalError;
        public static string FatalMessage;

        public static void Load(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;

            Config.Settings.ModSettings.Init(modEntry);
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUIWrapper;

            new AreaUnloadWatcher();

            Harmony = new Harmony(modEntry.Info.Id);
            Harmony.PatchAll();

            modEntry.Logger.Log("RandomReinforcementsPerEncounter loaded with Harmony");
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value && !FatalError;
            return true;
        }

        private static void OnGUIWrapper(UnityModManager.ModEntry entry)
        {
            if (FatalError)
            {
                GUILayout.BeginVertical("box");
                var boldWrap = new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Bold };
                var wrap = new GUIStyle(GUI.skin.label) { wordWrap = true };

                GUILayout.Label(
                    "Random Reinforcements per Encounter has been disabled due to a GUID collision or initialization error.",
                    boldWrap, GUILayout.Width(600f)
                );

                if (!string.IsNullOrEmpty(FatalMessage))
                {
                    GUILayout.Space(6);
                    GUILayout.Label(FatalMessage, wrap, GUILayout.Width(600f));
                }

                GUILayout.EndVertical();
                return;
            }
            
            ModUI.OnGUI(entry);
        }

        public static void FailMod(string message)
        {
            FatalError = true;
            FatalMessage = message ?? "Unknown error.";
            Enabled = false;

            if (ModEntry != null)
            {
                try { ModEntry.Logger.Error(message); } catch { }

                try { Harmony?.UnpatchAll(ModEntry.Info.Id); } catch { }

                try
                {
                    if (ModEntry.OnToggle != null)
                        ModEntry.OnToggle(ModEntry, false); 

                    ModEntry.Enabled = false;
                }
                catch { /* ok */ }
            }
        }
    }
}
