/*using System.Linq;
using UnityEngine;
using UnityModManagerNet;
using RandomReinforcementsPerEncounter.GameApi.Loot;

namespace RandomReinforcementsPerEncounter.UI.Pages
{
    /// <summary>
    /// Loot configuration — Phase 1 (UI only, not wired to config yet).
    /// </summary>
    internal static class LootConfigPage
    {
        private static GUIStyle _bold, _wrap, _right, _smallGray, _cell, _cellHeader;

        // ------ Sliders 1..1000% (100% = vanilla)
        private const float MinPct1000 = 1f;
        private const float MaxPct1000 = 1000f;
        private const float DefaultPct100 = 100f;

        // Estado temporal (no persistido en Fase 1)
        private static float _goldDropPct = DefaultPct100;
        private static float _genItemValuePct = DefaultPct100;

        // ------ Probabilidades 0..100%
        private const float MinPct100 = 0f;
        private const float MaxPct100 = 100f;

        private static float _weaponDropPct = 70f; // "Drop a weapon"
        private static float _oversizedPct = 15f;
        private static float _qualityMaterialPct = 20f;
        private static float _compositePct = 50f; // solo arcos
        private static float _masterworkPct = 30f;
        private static float _magicPct = 5f;

        // Distribución de materiales de calidad (suma 100%)
        private static float _matColdIron = 50f;
        private static float _matMithral = 30f;
        private static float _matAdamantite = 10f;
        private static float _matDruchite = 10f;
        private static int _matLastEdited = -1;

        // ------ Tiers (visual)
        private const int TierVisualMin = 1;
        private const int TierVisualMax = 20;
        private static readonly float[] _tierVisual = { 1, 1, 1, 1, 1, 1 }; // T1..T6

        // Tabla Tiers vs CR
        //private static Vector2 _tierTableScroll = Vector2.zero;
        private const int MaxCR = 30;

        private static void EnsureStyles()
        {
            if (_bold != null) return;

            _bold = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
            _wrap = new GUIStyle(GUI.skin.label) { wordWrap = true };
            _right = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight };

            _smallGray = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.Max(10, GUI.skin.label.fontSize - 2),
                normal = { textColor = new Color(1f, 1f, 1f, 0.70f) }
            };

            _cell = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleRight,
                padding = new RectOffset(4, 4, 2, 2)
            };

            _cellHeader = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(4, 4, 2, 4)
            };
        }

        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            EnsureStyles();

            GUILayout.Space(12);
            GUILayout.Label("Loot configuration", _bold);

            DrawSeparator();

            // ----------- Rates & multipliers -----------
            GUILayout.Label("Rates and multipliers", _bold);
            GUILayout.Space(4);
            GUILayout.Label("Ajusta los porcentajes. 100% = valores vanilla. UI provisional.", _wrap, GUILayout.Width(520f));

            GUILayout.Space(8);
            _goldDropPct = PercentSliderRow1000(
                "Gold drop amount",
                _goldDropPct,
                "Afecta al oro encontrado en cofres y enemigos. 100% = vanilla.");

            _genItemValuePct = PercentSliderRow1000(
                "Generated item value",
                _genItemValuePct,
                "Afecta al valor de mercado de los objetos mágicos generados por el mod. 100% = vanilla.");

            DrawSeparator();

            // ----------- Weapon drop configuration -----------
            GUILayout.Label("Weapon drop configuration", _bold);
            GUILayout.Space(4);

            _weaponDropPct = PercentSliderRow100(
                "Drop a weapon",
                _weaponDropPct,
                "Probabilidad de que el botín sea un arma.");

            GUILayout.Space(6);
            using (new GUILayout.VerticalScope("box"))
            {
                GUILayout.Label("If weapon is dropped", _bold);

                _oversizedPct = PercentSliderRow100("Oversized",
                    _oversizedPct,
                    "Probabilidad de que el arma sea oversized.");

                _qualityMaterialPct = PercentSliderRow100("Quality material",
                    _qualityMaterialPct,
                    "Probabilidad de que el arma use un material especial.");

                GUILayout.Space(4);
                GUILayout.Label("Quality material distribution (must sum 100%)", _bold);
                GUILayout.Label("Se reequilibra automáticamente para mantener 100% total.", _smallGray);
                DrawMaterialDistribution();

                _compositePct = PercentSliderRow100("Composite (bows only)",
                    _compositePct,
                    "Probabilidad de que el arco sea composite. Solo aplica a arcos.");

                _masterworkPct = PercentSliderRow100("Masterwork",
                    _masterworkPct,
                    "Probabilidad de que el arma sea masterwork.");

                _magicPct = PercentSliderRow100("Magic",
                    _magicPct,
                    "Probabilidad de que el arma tenga propiedades mágicas.");
            }

            DrawSeparator();

            // ----------- Tier preview (visual) -----------
            GUILayout.Label("Tier preview (visual — not applied)", _bold);
            GUILayout.Label("Seis barras (T1–T6) de 1 a 20. Solo UI.", _smallGray);
            DrawTierSlidersGrid();

            GUILayout.Space(6);
            GUILayout.Label("Tier chances by CR (calculated via TierChances.CalcTierChances)", _bold);
            GUILayout.Label("Valores mostrados en % (normalizados por fila). Tooltip con pesos crudos.", _smallGray);
            DrawTierTable();

            GUILayout.Space(6);
            GUILayout.Label("Nota: En la siguiente fase se enlazarán estos controles con la configuración real.", _wrap);
        }

        // ================== UI helpers ==================

        private static float PercentSliderRow1000(string label, float value, string tooltip)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(new GUIContent(label, tooltip), GUILayout.Width(230f));
                float newValue = GUILayout.HorizontalSlider(value, MinPct1000, MaxPct1000, GUILayout.Width(340f));
                newValue = Mathf.Clamp(Mathf.Round(newValue), MinPct1000, MaxPct1000);
                GUILayout.Label($"{newValue:0}% (×{newValue / 100f:0.00})", _right, GUILayout.Width(130f));
                return newValue;
            }
        }

        private static float PercentSliderRow100(string label, float value, string tooltip)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(new GUIContent(label, tooltip), GUILayout.Width(230f));
                float newValue = GUILayout.HorizontalSlider(value, MinPct100, MaxPct100, GUILayout.Width(340f));
                newValue = Mathf.Clamp(Mathf.Round(newValue), MinPct100, MaxPct100);
                GUILayout.Label($"{newValue:0}%", _right, GUILayout.Width(130f));
                return newValue;
            }
        }

        private static void DrawMaterialDistribution()
        {
            float prevCold = _matColdIron, prevMith = _matMithral, prevAdam = _matAdamantite, prevDruch = _matDruchite;

            _matColdIron = MaterialSliderRow("Cold iron", _matColdIron, 0);
            _matMithral = MaterialSliderRow("Mithral", _matMithral, 1);
            _matAdamantite = MaterialSliderRow("Adamantite", _matAdamantite, 2);
            _matDruchite = MaterialSliderRow("Druchite", _matDruchite, 3);

            if (!Mathf.Approximately(prevCold, _matColdIron)) _matLastEdited = 0;
            else if (!Mathf.Approximately(prevMith, _matMithral)) _matLastEdited = 1;
            else if (!Mathf.Approximately(prevAdam, _matAdamantite)) _matLastEdited = 2;
            else if (!Mathf.Approximately(prevDruch, _matDruchite)) _matLastEdited = 3;

            NormalizeMaterialsTo100(_matLastEdited);

            float sum = _matColdIron + _matMithral + _matAdamantite + _matDruchite;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label($"Total: {sum:0}%", _right, GUILayout.Width(130f));
            }
        }

        private static float MaterialSliderRow(string label, float value, int index)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Space(14f);
                GUILayout.Label(label, GUILayout.Width(216f)); // 230 - 14 indent
                float newValue = GUILayout.HorizontalSlider(value, 0f, 100f, GUILayout.Width(340f));
                newValue = Mathf.Clamp(Mathf.Round(newValue), 0f, 100f);
                GUILayout.Label($"{newValue:0}%", _right, GUILayout.Width(130f));
                return newValue;
            }
        }

        private static void NormalizeMaterialsTo100(int editedIndex)
        {
            _matColdIron = Mathf.Clamp(_matColdIron, 0f, 100f);
            _matMithral = Mathf.Clamp(_matMithral, 0f, 100f);
            _matAdamantite = Mathf.Clamp(_matAdamantite, 0f, 100f);
            _matDruchite = Mathf.Clamp(_matDruchite, 0f, 100f);

            float[] v = { _matColdIron, _matMithral, _matAdamantite, _matDruchite };

            if (editedIndex < 0 || editedIndex > 3) editedIndex = 0;

            float edited = v[editedIndex];
            float othersSum = 0f;
            for (int i = 0; i < 4; i++) if (i != editedIndex) othersSum += v[i];

            float targetOthers = 100f - edited;
            if (othersSum <= 0.0001f)
            {
                float share = targetOthers / 3f;
                for (int i = 0; i < 4; i++) if (i != editedIndex) v[i] = share;
            }
            else
            {
                float scale = targetOthers / (othersSum <= 0f ? 1f : othersSum);
                for (int i = 0; i < 4; i++)
                    if (i != editedIndex) v[i] = Mathf.Clamp(v[i] * scale, 0f, 100f);
            }

            float total = v[0] + v[1] + v[2] + v[3];
            float diff = 100f - total;

            for (int i = 3; i >= 0 && Mathf.Abs(diff) > 0.0001f; i--)
            {
                if (i == editedIndex) continue;
                float candidate = v[i] + diff;
                if (candidate >= 0f && candidate <= 100f)
                {
                    v[i] = candidate;
                    diff = 0f;
                }
            }
            if (Mathf.Abs(diff) > 0.0001f)
                v[editedIndex] = Mathf.Clamp(v[editedIndex] + diff, 0f, 100f);

            _matColdIron = v[0];
            _matMithral = v[1];
            _matAdamantite = v[2];
            _matDruchite = v[3];
        }

        // --------- Tiers (visual) ----------

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
                        GUILayout.Label($"{newVal:0}", _cell, GUILayout.Width(valW));
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
                GUILayout.FlexibleSpace(); // ← centra la tabla

                using (new GUILayout.VerticalScope("box"))
                {
                    // Cabecera (sin ancho fijo; solo mínimos para alineación)
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("CR", _cellHeader, GUILayout.MinWidth(crMinW));
                        for (int t = 0; t < 6; t++)
                            GUILayout.Label($"Tier {t + 1}", _cellHeader, GUILayout.MinWidth(tierMinW));
                    }

                    // 30 filas CR-1..CR-30
                    for (int cr = 1; cr <= MaxCR; cr++)
                    {
                        int[] weights = TierChances.CalcTierChances(cr);
                        int total = weights.Sum();

                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label($"CR-{cr}", _cell, GUILayout.MinWidth(crMinW));

                            for (int t = 0; t < 6; t++)
                            {
                                float pct = total > 0 ? (100f * weights[t] / total) : 0f;
                                var content = new GUIContent($"{pct:0}%", $"w={weights[t]}, sum={total}");
                                GUILayout.Label(content, _cell, GUILayout.MinWidth(tierMinW));
                            }
                        }
                    }
                }

                GUILayout.FlexibleSpace(); // ← centra la tabla
            }
        }


        private static void DrawSeparator()
        {
            GUILayout.Space(6);
            var rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(1f), GUILayout.ExpandWidth(true));
            if (Event.current.type == EventType.Repaint)
            {
                Color c = GUI.color;
                GUI.color = new Color(1f, 1f, 1f, 0.2f);
                GUI.DrawTexture(rect, Texture2D.whiteTexture);
                GUI.color = c;
            }
            GUILayout.Space(6);
        }
    }
}
*/