using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RandomReinforcementsPerEncounter
{
    internal static class AutoLinker
    {
        private const string ENC = "Encyclopedia:";
        private static string G(string key, string txt) => $"{{g|{key}}}{txt}{{/g}}";

        // Reglas (orden importa: de más específicas a más genéricas)
        private static readonly List<Func<string, string>> Rules = new List<Func<string, string>>()
    {
        // 1) Saving throw: "fortitude/reflex/will saving throw"
        s => Regex.Replace(s,
            @"\b(?i)(fortitude|reflex|will)\s+saving throw\b",
            m => G(ENC + "Saving_Throw", m.Value.ToLowerInvariant()),
            RegexOptions.CultureInvariant),

        // 2) DC suelto
        s => Regex.Replace(s,
            @"\bDC\b",
            m => G(ENC + "DC", m.Value),
            RegexOptions.CultureInvariant),

        // 3) Dados tipo "1d6", "2d10"
        s => Regex.Replace(s,
            @"\b\d+d\d+\b",
            m => G(ENC + "Dice", m.Value),
            RegexOptions.CultureInvariant),

        // 4) Round/rounds
        s => Regex.Replace(s,
            @"\brounds?\b",
            m => G(ENC + "Combat_Round", m.Value),
            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase),

        // 5) Energías (enlaza a la entrada genérica de daño de energía)
        s => Regex.Replace(s,
            @"\b(negative energy|electricity|acid|fire|cold|sonic|holy)\b",
            m => G(ENC + "Energy_Damage", m.Value),
            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase),

        // 6) Condiciones comunes (ajusta/añade las que uses)
        s => Regex.Replace(s,
            @"\b(blinded|shaken|frightened|stunned|dazed|sickened|nauseated|paralyzed|petrified|prone|entangled|exhausted|fatigued|confused|poisoned|diseased)\b",
            m => G(ENC + MapConditionKey(m.Value), m.Value),
            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase),
    };

        // Mapea texto -> key de enciclopedia (ej. "blinded" -> "ConditionBlind")
        private static string MapConditionKey(string conditionLower)
        {
            switch (conditionLower.ToLowerInvariant())
            {
                case "blinded": return "ConditionBlind";
                case "shaken": return "ConditionShaken";
                case "frightened": return "ConditionFrightened";
                case "stunned": return "ConditionStunned";
                case "dazed": return "ConditionDaze";
                case "sickened": return "ConditionSickened";
                case "nauseated": return "ConditionNauseated";
                case "paralyzed": return "ConditionParalyzed";
                case "petrified": return "ConditionPetrified";
                case "prone": return "ConditionProne";
                case "entangled": return "ConditionEntangled";
                case "exhausted": return "ConditionExhausted";
                case "fatigued": return "ConditionFatigued";
                case "confused": return "ConditionConfused";
                case "poisoned": return "ConditionPoisoned";
                case "diseased": return "ConditionDiseased";
                default: return "Condition"; // fallback genérico
            }
        }

        /// Aplica reglas evitando tocar texto que ya esté dentro de {g|...}{/g}
        public static string Apply(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var sb = new StringBuilder(input.Length + 64);
            int i = 0;

            while (i < input.Length)
            {
                int start = input.IndexOf("{g|", i, StringComparison.Ordinal);
                if (start < 0)
                {
                    sb.Append(ApplyRules(input.Substring(i)));
                    break;
                }

                // trozo fuera de link → aplicar reglas
                if (start > i) sb.Append(ApplyRules(input.Substring(i, start - i)));

                int end = input.IndexOf("{/g}", start, StringComparison.Ordinal);
                if (end < 0)
                {
                    // bloque sin cierre: por seguridad no aplicamos reglas al resto
                    sb.Append(input.Substring(start));
                    break;
                }

                // copiar bloque linkeado tal cual
                int len = end + 4 - start;
                sb.Append(input.Substring(start, len));
                i = end + 4;
            }

            return sb.ToString();
        }

        private static string ApplyRules(string segment)
        {
            string s = segment;
            for (int r = 0; r < Rules.Count; r++)
                s = Rules[r](s);
            return s;
        }
    }
}
