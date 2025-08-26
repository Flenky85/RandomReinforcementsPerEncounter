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
            //"Loot configuration",
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
                case 0: Pages.SpawnerConfigPage.Draw(modEntry); break;
                case 1: Pages.EnchantEncyclopediaPage.Draw(modEntry); break;
                //case 2: Pages.LootConfigPage.Draw(modEntry); break;
            }

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
