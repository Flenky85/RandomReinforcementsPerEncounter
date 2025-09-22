using UnityEngine;
using RandomReinforcementsPerEncounter.Config.Settings;

namespace RandomReinforcementsPerEncounter.UI.Pages.LootConfigParts
{
    internal static class WeaponDropSection
    {
        private const float MinPct100 = 0f;
        private const float MaxPct100 = 100f;

        private static int _matLastEdited = -1;

        public static void Draw()
        {
            var s = ModSettings.Instance;
            bool changed = false;

            GUILayout.Label("Weapon drop configuration", LootConfigPage.Bold);
            GUILayout.Space(4);

            changed |= SetIfChanged(ref s.WeaponDropPct, PercentSliderRow100(
                "Drop a weapon", s.WeaponDropPct, "Probabilidad de que el botín sea un arma."));

            GUILayout.Space(6);
            using (new GUILayout.VerticalScope("box"))
            {
                GUILayout.Label("If weapon is dropped", LootConfigPage.Bold);

                changed |= SetIfChanged(ref s.OversizedPct, PercentSliderRow100(
                    "Oversized", s.OversizedPct, "Probabilidad de que el arma sea oversized."));

                changed |= SetIfChanged(ref s.QualityMaterialPct, PercentSliderRow100(
                    "Quality material", s.QualityMaterialPct, "Probabilidad de que el arma use un material especial."));

                GUILayout.Space(4);
                GUILayout.Label("Quality material distribution (must sum 100%)", LootConfigPage.Bold);
                GUILayout.Label("Se reequilibra automáticamente para mantener 100% total.", LootConfigPage.SmallGray);

                changed |= DrawMaterialDistribution();

                changed |= SetIfChanged(ref s.CompositePct, PercentSliderRow100(
                    "Composite (bows only)", s.CompositePct, "Probabilidad de que el arco sea composite. Solo aplica a arcos."));

                changed |= SetIfChanged(ref s.MasterworkPct, PercentSliderRow100(
                    "Masterwork", s.MasterworkPct, "Probabilidad de que el arma sea masterwork."));

                changed |= SetIfChanged(ref s.MagicPct, PercentSliderRow100(
                    "Magic", s.MagicPct, "Probabilidad de que el arma tenga propiedades mágicas."));
            }

            GUILayout.Space(8);
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Restore weapon defaults", GUILayout.Width(220f)))
                {
                    ModSettings.ResetToDefaultsWeapons();
                    changed = false; // guardado dentro
                }
            }

            if (changed)
            {
                // sanear y persistir
                ModSettings.NormalizeMaterialsTo100(_matLastEdited < 0 ? 0 : _matLastEdited);
                ModSettings.Save();
            }
        }

        private static bool SetIfChanged(ref float field, float newValue)
        {
            if (!Mathf.Approximately(field, newValue))
            {
                field = Mathf.Clamp(Mathf.Round(newValue), MinPct100, MaxPct100);
                return true;
            }
            return false;
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

        private static bool DrawMaterialDistribution()
        {
            var s = ModSettings.Instance;
            bool changed = false;

            float prevCold = s.MatColdIron, prevMith = s.MatMithral, prevAdam = s.MatAdamantite, prevDruch = s.MatDruchite;

            s.MatColdIron = MaterialSliderRow("Cold iron", s.MatColdIron);
            s.MatMithral = MaterialSliderRow("Mithral", s.MatMithral);
            s.MatAdamantite = MaterialSliderRow("Adamantite", s.MatAdamantite);
            s.MatDruchite = MaterialSliderRow("Druchite", s.MatDruchite);

            if (!Mathf.Approximately(prevCold, s.MatColdIron)) _matLastEdited = 0;
            else if (!Mathf.Approximately(prevMith, s.MatMithral)) _matLastEdited = 1;
            else if (!Mathf.Approximately(prevAdam, s.MatAdamantite)) _matLastEdited = 2;
            else if (!Mathf.Approximately(prevDruch, s.MatDruchite)) _matLastEdited = 3;

            if (!Mathf.Approximately(prevCold, s.MatColdIron) ||
                !Mathf.Approximately(prevMith, s.MatMithral) ||
                !Mathf.Approximately(prevAdam, s.MatAdamantite) ||
                !Mathf.Approximately(prevDruch, s.MatDruchite))
            {
                changed = true;
            }

            // Mostrar suma
            float sum = s.MatColdIron + s.MatMithral + s.MatAdamantite + s.MatDruchite;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label($"Total: {sum:0}%", LootConfigPage.Right, GUILayout.Width(130f));
            }

            return changed;
        }

        private static float MaterialSliderRow(string label, float value)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Space(14f);
                GUILayout.Label(label, GUILayout.Width(216f)); 
                float newValue = GUILayout.HorizontalSlider(value, 0f, 100f, GUILayout.Width(340f));
                newValue = Mathf.Clamp(Mathf.Round(newValue), 0f, 100f);
                GUILayout.Label($"{newValue:0}%", LootConfigPage.Right, GUILayout.Width(130f));
                return newValue;
            }
        }
    }
}
