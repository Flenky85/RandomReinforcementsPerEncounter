using System.IO;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace RandomReinforcementsPerEncounter
{
    public class ModSettings
    {
        public int EncounterDifficultyModifier = 0;      // de -20 a +20
        public float PartyDifficultyOffset = 0f;         // de -10.0 a +10.0
        public int VariabilityMode = 0;                  // 0 = ambos, 1 = solo abajo, 2 = solo arriba
        public int VariabilityRange = 0;                 // de -20 a +20

        private static string _settingsPath;

        public static ModSettings Instance { get; private set; } = new ModSettings();

        public static void Init(UnityModManager.ModEntry modEntry)
        {
            _settingsPath = Path.Combine(modEntry.Path, "Settings.xml");

            if (File.Exists(_settingsPath))
            {
                try
                {
                    using (var reader = new StreamReader(_settingsPath))
                    {
                        var serializer = new XmlSerializer(typeof(ModSettings));
                        Instance = serializer.Deserialize(reader) as ModSettings ?? new ModSettings();
                    }
                }
                catch
                {
                    modEntry.Logger.Log("Error cargando Settings.xml, usando configuración por defecto.");
                    Instance = new ModSettings();
                }
            }
        }

        public static void Save()
        {
            if (_settingsPath == null) return;

            try
            {
                using (var writer = new StreamWriter(_settingsPath))
                {
                    var serializer = new XmlSerializer(typeof(ModSettings));
                    serializer.Serialize(writer, Instance);
                }
            }
            catch
            {
                // opcional: puedes loguear si quieres
            }
        }
    }
}

