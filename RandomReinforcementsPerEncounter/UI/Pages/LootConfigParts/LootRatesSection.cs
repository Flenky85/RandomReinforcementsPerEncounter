using UnityEngine;

namespace RandomReinforcementsPerEncounter.UI.Pages.LootConfigParts
{
    /// <summary>
    /// Sección: sliders de 1..1000% para oro dropeado y valor de ítems generados.
    /// </summary>
    internal static class LootRatesSection
    {
        private const float MinPct1000 = 1f;
        private const float MaxPct1000 = 1000f;
        private const float DefaultPct100 = 100f;

        // Estado temporal (no persistido en Fase 1)
        private static float _goldDropPct = DefaultPct100;
        private static float _genItemValuePct = DefaultPct100;

        public static void Draw()
        {
            GUILayout.Label("Rates and multipliers", LootConfigPage.Bold);
            GUILayout.Space(4);
            GUILayout.Label("Ajusta los porcentajes. 100% = valores vanilla. UI provisional.", LootConfigPage.Wrap, GUILayout.Width(520f));

            GUILayout.Space(8);
            _goldDropPct = PercentSliderRow1000(
                "Gold drop amount",
                _goldDropPct,
                "Afecta al oro encontrado en cofres y enemigos. 100% = vanilla.");

            _genItemValuePct = PercentSliderRow1000(
                "Generated item value",
                _genItemValuePct,
                "Afecta al valor de mercado de los objetos mágicos generados por el mod. 100% = vanilla.");
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
