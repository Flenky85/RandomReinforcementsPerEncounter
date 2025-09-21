using RandomReinforcementsPerEncounter.UI.Pages;
using UnityEngine;
using UnityModManagerNet;

namespace RandomReinforcementsPerEncounter.UI
{
    internal static class ModUI
    {
        private static int _tabIndex = 0;
        private static readonly string[] Tabs =
        {
            "Spawner configuration",
            "Loot configuration",
            "Enchant encyclopedia"
        };

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUILayout.Width(560f));

            GUILayout.Space(10);
            _tabIndex = GUILayout.Toolbar(_tabIndex, Tabs, GUILayout.Width(560f));
            GUILayout.Space(10);

            switch (_tabIndex)
            {
                case 0: SpawnerConfigPage.Draw(modEntry); break;
                case 1: LootConfigPage.Draw(modEntry); break;
                case 2: EnchantEncyclopediaPage.Draw(modEntry); break;
            }

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
