using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    public static class BlueprintDumper
    {
        private static HashSet<object> _visited = new HashSet<object>();

        public static void DumpObject(object obj, int depth = 2, string prefix = "[Dump]")
        {
            _visited.Clear();
            Dump(obj, 0, depth, prefix);
        }

        private static void Dump(object obj, int level, int maxDepth, string prefix)
        {
            if (obj == null)
            {
                Debug.Log($"{prefix} {new string(' ', level * 2)}null");
                return;
            }

            if (_visited.Contains(obj))
            {
                Debug.Log($"{prefix} {new string(' ', level * 2)}(referencia repetida: {obj.GetType().Name})");
                return;
            }

            _visited.Add(obj);

            Type type = obj.GetType();
            Debug.Log($"{prefix} {new string(' ', level * 2)}{type.Name}");

            if (level >= maxDepth || type.IsPrimitive || obj is string || obj is Enum)
            {
                Debug.Log($"{prefix} {new string(' ', (level + 1) * 2)}{obj}");
                return;
            }

            if (obj is IEnumerable enumerable && !(obj is string))
            {
                int i = 0;
                foreach (var item in enumerable)
                {
                    Debug.Log($"{prefix} {new string(' ', (level + 1) * 2)}[{i++}]");
                    Dump(item, level + 2, maxDepth, prefix);
                }
                return;
            }

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                object value = null;
                try { value = field.GetValue(obj); }
                catch { }

                Debug.Log($"{prefix} {new string(' ', (level + 1) * 2)}{field.Name} = {(value == null ? "null" : value.ToString())}");

                if (value != null && !(value is string) && !field.FieldType.IsPrimitive && !field.FieldType.IsEnum)
                {
                    Dump(value, level + 2, maxDepth, prefix);
                }
            }
        }
    }
}
