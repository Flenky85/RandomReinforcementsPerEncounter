using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityModManagerNet;

namespace RandomReinforcementsPerEncounter.Config.Settings
{
    [XmlRoot("ModSettings")]
    public class ModSettings
    {
        // Spawner options
        public int EncounterDifficultyModifier = 0;
        public float PartyDifficultyOffset = 0f;
        public int VariabilityMode = 0;
        public int VariabilityRange = 0;
        public bool monsterspawnerexp = false;
        public bool Chestspawn = true;
        public bool spawnerenable = true;

        // Gold options
        public float GoldDropPct = 100f;     
        public float GenItemValuePct = 100f; 

        // Weapon drop options
        public float WeaponDropPct = 70f;    
        public float OversizedPct = 15f;
        public float QualityMaterialPct = 20f;
        public float CompositePct = 50f;     
        public float MasterworkPct = 30f;
        public float MagicPct = 5f;

        // Quyality material options
        public float MatColdIron = 50f;
        public float MatMithral = 30f;
        public float MatAdamantite = 10f;
        public float MatDruchite = 10f;

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
                    NormalizeMaterialsTo100();
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
            // Spawner
            Instance.EncounterDifficultyModifier = Mathf.Clamp(Instance.EncounterDifficultyModifier, -20, 20);
            Instance.PartyDifficultyOffset = Mathf.Clamp(Instance.PartyDifficultyOffset, -10f, 10f);
            Instance.VariabilityMode = Mathf.Clamp(Instance.VariabilityMode, 0, 2);
            Instance.VariabilityRange = Mathf.Clamp(Instance.VariabilityRange, -20, 20);

            // Gold rates
            Instance.GoldDropPct = Mathf.Clamp(Mathf.Round(Instance.GoldDropPct), 1f, 1000f);
            Instance.GenItemValuePct = Mathf.Clamp(Mathf.Round(Instance.GenItemValuePct), 1f, 1000f);

            // Weapon drop
            Instance.WeaponDropPct = Clamp01pct(Instance.WeaponDropPct);
            Instance.OversizedPct = Clamp01pct(Instance.OversizedPct);
            Instance.QualityMaterialPct = Clamp01pct(Instance.QualityMaterialPct);
            Instance.CompositePct = Clamp01pct(Instance.CompositePct);
            Instance.MasterworkPct = Clamp01pct(Instance.MasterworkPct);
            Instance.MagicPct = Clamp01pct(Instance.MagicPct);

            // Materials
            Instance.MatColdIron = Clamp01pct(Instance.MatColdIron);
            Instance.MatMithral = Clamp01pct(Instance.MatMithral);
            Instance.MatAdamantite = Clamp01pct(Instance.MatAdamantite);
            Instance.MatDruchite = Clamp01pct(Instance.MatDruchite);
        }

        private static float Clamp01pct(float v) => Mathf.Clamp(Mathf.Round(v), 0f, 100f);

        public static void NormalizeMaterialsTo100(int editedIndex = 0)
        {
            float[] v = {
                Mathf.Clamp(Instance.MatColdIron, 0f, 100f),
                Mathf.Clamp(Instance.MatMithral, 0f, 100f),
                Mathf.Clamp(Instance.MatAdamantite, 0f, 100f),
                Mathf.Clamp(Instance.MatDruchite, 0f, 100f)
            };
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

            Instance.MatColdIron = v[0];
            Instance.MatMithral = v[1];
            Instance.MatAdamantite = v[2];
            Instance.MatDruchite = v[3];
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

        public static void ResetToDefaultsWeapons()
        {
            var d = new ModSettings();
            Instance.WeaponDropPct = d.WeaponDropPct;
            Instance.OversizedPct = d.OversizedPct;
            Instance.QualityMaterialPct = d.QualityMaterialPct;
            Instance.CompositePct = d.CompositePct;
            Instance.MasterworkPct = d.MasterworkPct;
            Instance.MagicPct = d.MagicPct;

            Instance.MatColdIron = d.MatColdIron;
            Instance.MatMithral = d.MatMithral;
            Instance.MatAdamantite = d.MatAdamantite;
            Instance.MatDruchite = d.MatDruchite;

            NormalizeMaterialsTo100();
            Save();
        }
    }
}
