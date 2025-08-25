using UnityEngine;
using UnityModManagerNet;

namespace RandomReinforcementsPerEncounter.UI.Pages
{
    internal static class LootConfigPage
    {
        private static GUIStyle _bold, _wrap;

        private static void EnsureStyles()
        {
            if (_bold == null)
            {
                _bold = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
                _wrap = new GUIStyle(GUI.skin.label) { wordWrap = true };
            }
        }

        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            EnsureStyles();

            GUILayout.Space(15);
            GUILayout.Label("Loot configuration", _bold);
            GUILayout.Space(5);
            GUILayout.Label(
                "Coming soon: Loot configuration.\n\n" +
                "You’ll be able to customize:\n" +
                "• How much gold appears in chests and on enemies.\n" +
                "• Price multipliers for the randomly enchanted magic items created by the mod.\n" +
                "• Chances/weights to get one enchant or another (e.g., Flaming vs. Frost).\n\n" +
                "This page is a stub for now — settings will be added in the next update.",
                _wrap, GUILayout.Width(500f)
            );
        }
    }
}
