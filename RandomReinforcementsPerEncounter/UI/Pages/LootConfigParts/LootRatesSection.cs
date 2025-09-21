// File: UI/Pages/LootConfigParts/LootRatesSection.cs
using UnityEngine;
using RandomReinforcementsPerEncounter.Config.Settings;

namespace RandomReinforcementsPerEncounter.UI.Pages.LootConfigParts
{
    /// <summary>
    /// Sección: sliders de 1..1000% para oro dropeado y valor de ítems generados.
    /// </summary>
    internal static class GoldRatesSection
    {
        private const float MinPct1000 = 1f;
        private const float MaxPct1000 = 1000f;

        public static void Draw()
        {
            var s = ModSettings.Instance;
            bool changed = false;

            GUILayout.Label("Rates and multipliers", LootConfigPage.Bold);
            GUILayout.Space(4);
            GUILayout.Label("Ajusta los porcentajes. 100% = valores vanilla.", LootConfigPage.Wrap, GUILayout.Width(520f));

            GUILayout.Space(8);
            float newGold = PercentSliderRow1000(
                "Gold drop amount",
                s.GoldDropPct,
                "Afecta al oro encontrado en cofres y enemigos. 100% = vanilla."
            );
            if (!Mathf.Approximately(newGold, s.GoldDropPct))
            {
                s.GoldDropPct = newGold;
                changed = true;
            }

            float newGenVal = PercentSliderRow1000(
                "Generated item value",
                s.GenItemValuePct,
                "Afecta al valor de mercado de los objetos mágicos generados por el mod. 100% = vanilla."
            );
            if (!Mathf.Approximately(newGenVal, s.GenItemValuePct))
            {
                s.GenItemValuePct = newGenVal;
                changed = true;
            }

            GUILayout.Space(10);
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Restore loot defaults", GUILayout.Width(200)))
                {
                    ModSettings.ResetToDefaultsGold();
                    changed = false;
                }
            }

            if (changed) ModSettings.Save();
        }

        private static float PercentSliderRow1000(string label, float value, string tooltip)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(new GUIContent(label, tooltip), GUILayout.Width(230f));
                float newValue = GUILayout.HorizontalSlider(value, MinPct1000, MaxPct1000, GUILayout.Width(340f));
                newValue = Mathf.Clamp(Mathf.Round(newValue), MinPct1000, MaxPct1000);
                GUILayout.Label($"{newValue:0}% (×{newValue / 100f:0.00})", LootConfigPage.Right, GUILayout.Width(130f));
                return newValue;
            }
        }
    }
}
