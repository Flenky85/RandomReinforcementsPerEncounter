using BlueprintCore.Utils;
using Kingmaker.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomReinforcementsPerEncounter
{
    internal static class KeyBuilderUtils
    {
        public static (string nameKey, string descKey, string bpName, LocalizedString locName)
            BuildKeys(string nameRoot, int tierIndex, string name)
        {
            string nameKey = $"RRE.{nameRoot}.T{tierIndex}.Name";
            string descKey = $"RRE.{nameRoot}.T{tierIndex}.Desc";
            string bpName = $"RRE_{nameRoot}_T{tierIndex}_Enchant";
            var locName = LocalizationTool.CreateString(nameKey, $"{name} (T{tierIndex})");

            return (nameKey, descKey, bpName, locName);
        }
    }
}
