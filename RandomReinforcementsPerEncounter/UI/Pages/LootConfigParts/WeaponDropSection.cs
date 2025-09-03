using UnityEngine;

namespace RandomReinforcementsPerEncounter.UI.Pages.LootConfigParts
{
    /// <summary>
    /// Sección: probabilidades de dropear arma y subopciones (0..100%).
    /// Incluye distribución de materiales que siempre suma 100%.
    /// </summary>
    internal static class WeaponDropSection
    {
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

        public static void Draw()
        {
            GUILayout.Label("Weapon drop configuration", LootConfigPage.Bold);
            GUILayout.Space(4);

            _weaponDropPct = PercentSliderRow100(
                "Drop a weapon",
                _weaponDropPct,
                "Probabilidad de que el botín sea un arma.");

            GUILayout.Space(6);
            using (new GUILayout.VerticalScope("box"))
            {
                GUILayout.Label("If weapon is dropped", LootConfigPage.Bold);

                _oversizedPct = PercentSliderRow100(
                    "Oversized",
                    _oversizedPct,
                    "Probabilidad de que el arma sea oversized.");

                _qualityMaterialPct = PercentSliderRow100(
                    "Quality material",
                    _qualityMaterialPct,
                    "Probabilidad de que el arma use un material especial.");

                GUILayout.Space(4);
                GUILayout.Label("Quality material distribution (must sum 100%)", LootConfigPage.Bold);
                GUILayout.Label("Se reequilibra automáticamente para mantener 100% total.", LootConfigPage.SmallGray);

                DrawMaterialDistribution();

                _compositePct = PercentSliderRow100(
                    "Composite (bows only)",
                    _compositePct,
                    "Probabilidad de que el arco sea composite. Solo aplica a arcos.");

                _masterworkPct = PercentSliderRow100(
                    "Masterwork",
                    _masterworkPct,
                    "Probabilidad de que el arma sea masterwork.");

                _magicPct = PercentSliderRow100(
                    "Magic",
                    _magicPct,
                    "Probabilidad de que el arma tenga propiedades mágicas.");
            }
        }

        private static float PercentSliderRow100(string label, float value, string tooltip)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(new GUIContent(label, tooltip), GUILayout.Width(230f));
                float newValue = GUILayout.HorizontalSlider(value, MinPct100, MaxPct100, GUILayout.Width(340f));
                newValue = Mathf.Clamp(Mathf.Round(newValue), MinPct100, MaxPct100);
                GUILayout.Label($"{newValue:0}%", LootConfigPage.Right, GUILayout.Width(130f));
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
                GUILayout.Label($"Total: {sum:0}%", LootConfigPage.Right, GUILayout.Width(130f));
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
                GUILayout.Label($"{newValue:0}%", LootConfigPage.Right, GUILayout.Width(130f));
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
                float scale = targetOthers / othersSum;
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
    }
}
