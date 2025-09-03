using System.Linq;
using UnityEngine;
using RandomReinforcementsPerEncounter.GameApi.Loot;

namespace RandomReinforcementsPerEncounter.UI.Pages.LootConfigParts
{
    /// <summary>
    /// Sección: sliders visuales T1..T6 (1..20) y tabla CR-1..CR-30 con % por tier.
    /// </summary>
    internal static class TierPreviewSection
    {
        // Sliders visuales
        private const int TierVisualMin = 1;
        private const int TierVisualMax = 20;
        private static readonly float[] _tierVisual = { 1, 1, 1, 1, 1, 1 }; // T1..T6

        private const int MaxCR = 30;

        public static void Draw()
        {
            GUILayout.Label("Tier preview (visual — not applied)", LootConfigPage.Bold);
            GUILayout.Label("Seis barras (T1–T6) de 1 a 20. Solo UI.", LootConfigPage.SmallGray);
            DrawTierSlidersGrid();

            GUILayout.Space(6);
            GUILayout.Label("Tier chances by CR (calculated via TierChances.CalcTierChances)", LootConfigPage.Bold);
            GUILayout.Label("Valores mostrados en % (normalizados por fila). Tooltip con pesos crudos.", LootConfigPage.SmallGray);
            DrawTierTable();
        }

        private static void DrawTierSlidersGrid()
        {
            const float labelW = 70f;
            const float sliderW = 180f;
            const float valW = 40f;

            for (int row = 0; row < 2; row++)
            {
                using (new GUILayout.HorizontalScope())
                {
                    for (int col = 0; col < 3; col++)
                    {
                        int i = row * 3 + col; // 0..5
                        GUILayout.Label($"Tier {i + 1}", GUILayout.Width(labelW));
                        float newVal = GUILayout.HorizontalSlider(_tierVisual[i], TierVisualMin, TierVisualMax, GUILayout.Width(sliderW));
                        newVal = Mathf.Clamp(Mathf.Round(newVal), TierVisualMin, TierVisualMax);
                        GUILayout.Label($"{newVal:0}", LootConfigPage.Cell, GUILayout.Width(valW));
                        GUILayout.Space(12f);
                        _tierVisual[i] = newVal;
                    }
                }
            }
        }

        private static void DrawTierTable()
        {
            const float crMinW = 60f;
            const float tierMinW = 64f;

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace(); // centrar

                using (new GUILayout.VerticalScope("box"))
                {
                    // Cabecera
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("CR", LootConfigPage.CellHeader, GUILayout.MinWidth(crMinW));
                        for (int t = 0; t < 6; t++)
                            GUILayout.Label($"Tier {t + 1}", LootConfigPage.CellHeader, GUILayout.MinWidth(tierMinW));
                    }

                    // Filas CR-1..CR-30
                    for (int cr = 1; cr <= MaxCR; cr++)
                    {
                        int[] weights = TierChances.CalcTierChances(cr);
                        int total = weights.Sum();

                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label($"CR-{cr}", LootConfigPage.Cell, GUILayout.MinWidth(crMinW));

                            for (int t = 0; t < 6; t++)
                            {
                                float pct = total > 0 ? 100f * weights[t] / total : 0f;
                                var content = new GUIContent($"{pct:0}%", $"w={weights[t]}, sum={total}");
                                GUILayout.Label(content, LootConfigPage.Cell, GUILayout.MinWidth(tierMinW));
                            }
                        }
                    }
                }

                GUILayout.FlexibleSpace(); // centrar
            }
        }
    }
}
