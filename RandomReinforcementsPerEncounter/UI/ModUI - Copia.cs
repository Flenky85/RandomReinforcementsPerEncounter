/*using UnityEngine;
using UnityModManagerNet;
using RandomReinforcementsPerEncounter.Config.Settings;

namespace RandomReinforcementsPerEncounter.UI
{
    internal static class ModUI
    {
        private const float SliderWidth = 500f;

        // cache de estilos y textos
        private static GUIStyle _boldLabel, _centeredLabel;
        private static readonly string HelpText =
            "Let’s say your party has 4 characters, each at level 5.\n" +
            "And let’s say they are facing 3 dretches, each with CR 2.\n\n" +
            "- The average level of your party determines the CR of the monsters that will be spawned. In this case: 20 / 4 = 5.\n" +
            "- The total level of your party is used to measure the overall challenge budget. Here, that’s 20.\n" +
            "- The total CR of your current enemies: 3 dretches * CR 2 = 6.\n" +
            "- The remaining CR budget is 20 - 6 = 14.\n" +
            "- Since the CR of the monsters to be spawned is 5, we divide: 14 / 5 = 2.8, which always rounds up to 3.\n" +
            "- So, 3 enemies of CR 5 will be spawned.\n" +
            "- If your party’s CR is lower than the total CR of the enemies you're facing, no reinforcements will be spawned.\n\n";

        private static void EnsureStyles()
        {
            if (_boldLabel == null)
            {
                _boldLabel = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
                _centeredLabel = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true };
            }
        }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            EnsureStyles();

            var settings = ModSettings.Instance;
            bool changed = false;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();

            GUILayout.Space(15);
            GUILayout.Label("How the spawner works:", _boldLabel);
            GUILayout.Label(HelpText, GUILayout.Width(SliderWidth));

            // Encounter CR modifier
            GUILayout.Label("Increases or decreases your party’s average level to determine the CR of the summoned enemies:", GUILayout.Width(SliderWidth));
            int encounterMod = Mathf.RoundToInt(
                GUILayout.HorizontalSlider(settings.EncounterDifficultyModifier, -20, 20, GUILayout.Width(SliderWidth))
            );
            GUILayout.Label($"{encounterMod}", _centeredLabel, GUILayout.Width(SliderWidth));
            if (encounterMod != settings.EncounterDifficultyModifier)
            {
                settings.EncounterDifficultyModifier = encounterMod;
                changed = true;
            }

            GUILayout.Space(15);

            // Party difficulty offset
            GUILayout.Label("Adjusts the total party level used for the challenge budget calculation, which results in more or fewer enemies being spawned:", GUILayout.Width(SliderWidth));
            float partyOffset = Mathf.Round(
                GUILayout.HorizontalSlider(settings.PartyDifficultyOffset, -10f, 10f, GUILayout.Width(SliderWidth)) * 10f
            ) / 10f;
            GUILayout.Label($"{partyOffset * 100:+0;-0}%", _centeredLabel, GUILayout.Width(SliderWidth));
            if (!Mathf.Approximately(partyOffset, settings.PartyDifficultyOffset))
            {
                settings.PartyDifficultyOffset = partyOffset;
                changed = true;
            }

            GUILayout.Space(15);

            // Variability mode
            GUILayout.Label("Enables CR variation in summoned enemies, making them potentially stronger or weaker:", GUILayout.Width(SliderWidth));
            int mode = GUILayout.SelectionGrid(
                settings.VariabilityMode,
                new[] { "Stronger or weaker", "Only weaker", "Only stronger" },
                3,
                GUILayout.Width(SliderWidth)
            );
            if (mode != settings.VariabilityMode)
            {
                settings.VariabilityMode = mode;
                changed = true;
            }

            GUILayout.Space(5);

            // Variability range (always visible for clarity)
            GUILayout.Label("Defines the variability range within which enemy CR can fluctuate:", GUILayout.Width(SliderWidth));
            int variability = Mathf.RoundToInt(
                GUILayout.HorizontalSlider(settings.VariabilityRange, 0, 20, GUILayout.Width(SliderWidth))
            );
            GUILayout.Label($"{variability}", _centeredLabel, GUILayout.Width(SliderWidth));
            if (variability != settings.VariabilityRange)
            {
                settings.VariabilityRange = variability;
                changed = true;
            }

            GUILayout.Space(15);

            // (Opcional) Botón Reset
            // if (GUILayout.Button("Reset to Defaults", GUILayout.Width(SliderWidth)))
            // {
            //     ModSettings.Instance = new ModSettings();
            //     changed = true;
            // }

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (changed)
                ModSettings.Save();
        }
    }
}
*/