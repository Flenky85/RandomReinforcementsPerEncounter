using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityModManagerNet;

namespace RandomReinforcementsPerEncounter.Config.Settings
{
    [XmlRoot("ModSettings")]
    public class ModSettings
    {
        //Spawner options
        public int EncounterDifficultyModifier = 0;   
        public float PartyDifficultyOffset = 0f;      
        public int VariabilityMode = 0;               
        public int VariabilityRange = 0;
        public bool monsterspawnerexp = false; 
        public bool Chestspawn = true;         
        public bool spawnerenable = true;

        //Gold options
        public float GoldDropPct = 100f;        
        public float GenItemValuePct = 100f;    

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
                    Clamp(); 
                }
                catch
                {
                    modEntry.Logger.Log("Error cargando Settings.xml, usando configuración por defecto.");
                    Instance = new ModSettings();
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(modEntry.Path);
                    Save();
                }
                catch { }
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
            catch { }
        }

        private static void Clamp()
        {
            Instance.EncounterDifficultyModifier = Mathf.Clamp(Instance.EncounterDifficultyModifier, -20, 20);
            Instance.PartyDifficultyOffset = Mathf.Clamp(Instance.PartyDifficultyOffset, -10f, 10f);
            Instance.VariabilityMode = Mathf.Clamp(Instance.VariabilityMode, 0, 2);
            Instance.VariabilityRange = Mathf.Clamp(Instance.VariabilityRange, -20, 20);

            Instance.GoldDropPct = Mathf.Clamp(Mathf.Round(Instance.GoldDropPct), 1f, 1000f);
            Instance.GenItemValuePct = Mathf.Clamp(Mathf.Round(Instance.GenItemValuePct), 1f, 1000f);
        }
        public static void ResetToDefaultsSpawner()
        {
            var d = new ModSettings();
            Instance.EncounterDifficultyModifier = d.EncounterDifficultyModifier;
            Instance.PartyDifficultyOffset = d.PartyDifficultyOffset;
            Instance.VariabilityMode = d.VariabilityMode;
            Instance.VariabilityRange = d.VariabilityRange;
            Instance.monsterspawnerexp = d.monsterspawnerexp;
            Instance.Chestspawn = d.Chestspawn;
            Instance.spawnerenable = d.spawnerenable;
            Save();
        }
        public static void ResetToDefaultsGold()
        {
            var d = new ModSettings();
            Instance.GoldDropPct = d.GoldDropPct;
            Instance.GenItemValuePct = d.GenItemValuePct;
            Save();
        }
    }
}
