using RandomReinforcementsPerEncounter.UI.Pages.LootConfigParts;
using UnityEngine;
using UnityModManagerNet;

namespace RandomReinforcementsPerEncounter.UI.Pages
{
    /// <summary>
    /// Loot configuration — Phase 1 (UI only, not wired to config yet).
    /// Orquestador: pinta cabecera/separadores y delega en las tres secciones.
    /// </summary>
    internal static class LootConfigPage
    {
        // --- estilos compartidos (expuestos para que los usen las secciones) ---
        private static GUIStyle _bold, _wrap, _right, _smallGray, _cell, _cellHeader;

        internal static GUIStyle Bold => _bold;
        internal static GUIStyle Wrap => _wrap;
        internal static GUIStyle Right => _right;
        internal static GUIStyle SmallGray => _smallGray;
        internal static GUIStyle Cell => _cell;
        internal static GUIStyle CellHeader => _cellHeader;

        internal static void EnsureStyles()
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

        internal static void Separator()
        {
            GUILayout.Space(6);
            var rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(1f), GUILayout.ExpandWidth(true));
            if (Event.current.type == EventType.Repaint)
            {
                var c = GUI.color;
                GUI.color = new Color(1f, 1f, 1f, 0.2f);
                GUI.DrawTexture(rect, Texture2D.whiteTexture);
                GUI.color = c;
            }
            GUILayout.Space(6);
        }

        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            EnsureStyles();

            GUILayout.Space(12);
            GUILayout.Label("Loot configuration", Bold);
            Separator();

            // 1) Oro y valor de ítems
            GoldRatesSection.Draw();

            Separator();

            // 2) Configuración de dropeo de armas
            WeaponDropSection.Draw();

            Separator();

            // 3) Tiers (preview + tabla)
            TierPreviewSection.Draw();

            GUILayout.Space(6);
            GUILayout.Label("Nota: Fase 1 — UI sin persistencia ni aplicación.", Wrap);
        }
    }
}
