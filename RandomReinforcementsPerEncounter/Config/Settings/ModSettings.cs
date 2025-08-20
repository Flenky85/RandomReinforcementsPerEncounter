using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityModManagerNet;

namespace RandomReinforcementsPerEncounter.Config.Settings
{
    [XmlRoot("ModSettings")]
    public class ModSettings
    {
        public int EncounterDifficultyModifier = 0;   // -20..+20
        public float PartyDifficultyOffset = 0f;      // -10..+10
        public int VariabilityMode = 0;               // 0=ambos, 1=solo abajo, 2=solo arriba
        public int VariabilityRange = 0;              // -20..+20

        private static string _settingsPath;
        public static ModSettings Instance { get; private set; } = new ModSettings();

        public static void Init(UnityModManager.ModEntry modEntry)
        {
            _settingsPath = Path.Combine(modEntry.Path, "Settings.xml");

            if (File.Exists(_settingsPath))
            {
                try
                {
                    using var reader = new StreamReader(_settingsPath);
                    var serializer = new XmlSerializer(typeof(ModSettings));
                    Instance = serializer.Deserialize(reader) as ModSettings ?? new ModSettings();
                    Clamp(); // opcional: asegurar rangos tras cargar
                }
                catch
                {
                    modEntry.Logger.Log("Error cargando Settings.xml, usando configuración por defecto.");
                    Instance = new ModSettings();
                }
            }
            else
            {
                // Primera vez: crea el dir si hiciera falta y guarda por defecto
                try
                {
                    Directory.CreateDirectory(modEntry.Path);
                    Save();
                }
                catch { /* opcional log */ }
            }
        }

        public static void Save()
        {
            if (string.IsNullOrEmpty(_settingsPath)) return;

            try
            {
                using var writer = new StreamWriter(_settingsPath);
                var serializer = new XmlSerializer(typeof(ModSettings));
                serializer.Serialize(writer, Instance);
            }
            catch
            {
                // opcional: log
            }
        }

        // Opcional: validación de límites para evitar valores fuera de rango
        private static void Clamp()
        {
            Instance.EncounterDifficultyModifier = Mathf.Clamp(Instance.EncounterDifficultyModifier, -20, 20);
            Instance.PartyDifficultyOffset = Mathf.Clamp(Instance.PartyDifficultyOffset, -10f, 10f);
            Instance.VariabilityMode = Mathf.Clamp(Instance.VariabilityMode, 0, 2);
            Instance.VariabilityRange = Mathf.Clamp(Instance.VariabilityRange, -20, 20);
        }
    }
}
