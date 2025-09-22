using System.Linq;
using UnityEngine;
using RandomReinforcementsPerEncounter.GameApi.Loot;
using RandomReinforcementsPerEncounter.Config.Settings; // ← añade esto

namespace RandomReinforcementsPerEncounter.UI.Pages.LootConfigParts
{
    internal static class TierPreviewSection
    {
        private const int TierVisualMin = 1;
        private const int TierVisualMax = 20;
        private const int MaxCR = 30;

        public static void Draw()
        {
            GUILayout.Label("Tier preview (visual — not applied)", LootConfigPage.Bold);
            GUILayout.Label("Seis barras (T1–T6) de 1 a 20. Solo UI.", LootConfigPage.SmallGray);

            bool changed = DrawTierSlidersGrid();

            GUILayout.Space(6);
            GUILayout.Label("Tier chances by CR (calculated via TierChances.CalcTierChances)", LootConfigPage.Bold);
            GUILayout.Label("Valores mostrados en % (normalizados por fila). Tooltip con pesos crudos.", LootConfigPage.SmallGray);
            DrawTierTable();

            if (changed) ModSettings.Save();
        }

        private static bool DrawTierSlidersGrid()
        {
            const float labelW = 70f;
            const float sliderW = 180f;
            const float valW = 40f;

            var s = ModSettings.Instance;
            bool changed = false;

            // helper local
            int SliderRow(string label, int value)
            {
                GUILayout.Label(label, GUILayout.Width(labelW));
                float f = GUILayout.HorizontalSlider(value, TierVisualMin, TierVisualMax, GUILayout.Width(sliderW));
                int newVal = Mathf.Clamp(Mathf.RoundToInt(f), TierVisualMin, TierVisualMax);
                GUILayout.Label($"{newVal:0}", LootConfigPage.Cell, GUILayout.Width(valW));
                GUILayout.Space(12f);
                return newVal;
            }

            for (int row = 0; row < 2; row++)
            {
                using (new GUILayout.HorizontalScope())
                {
                    for (int col = 0; col < 3; col++)
                    {
                        int i = row * 3 + col; // 0..5
                        int before =
                            i == 0 ? s.TierVisual1 :
                            i == 1 ? s.TierVisual2 :
                            i == 2 ? s.TierVisual3 :
                            i == 3 ? s.TierVisual4 :
                            i == 4 ? s.TierVisual5 :
                                     s.TierVisual6;

                        int after = SliderRow($"Tier {i + 1}", before);

                        if (after != before)
                        {
                            switch (i)
                            {
                                case 0: s.TierVisual1 = after; break;
                                case 1: s.TierVisual2 = after; break;
                                case 2: s.TierVisual3 = after; break;
                                case 3: s.TierVisual4 = after; break;
                                case 4: s.TierVisual5 = after; break;
                                case 5: s.TierVisual6 = after; break;
                            }
                            changed = true;
                        }
                    }
                }
            }

            GUILayout.Space(6);
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Restore tier defaults", GUILayout.Width(200f)))
                {
                    ModSettings.ResetToDefaultsTiers();
                    changed = false; // ya guarda dentro
                }
            }

            return changed;
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
