using Kingmaker.Blueprints;

namespace RandomReinforcementsPerEncounter.Config.Ids
{
    /// <summary>
    /// Fuente única de GUIDs, en string, sin prefijos raros.
    /// Mantener EXACTAMENTE las mismas cadenas siempre (ver tu regla de estabilidad de GUIDs).
    /// </summary>
    internal static class BlueprintGuids
    {
        internal static class EnhancementPlus
        {
            // +1
            public const string WeaponPlus1 = "d42fc23b92c640846ac137dc26e000d4";
            public const string ArmorPlus1 = "a9ea95c5e02f9b7468447bc1010fe152";
            public const string ShieldPlus1 = "e90c252e08035294eba39bafce76c119";

            // +2
            public const string WeaponPlus2 = "eb2faccc4c9487d43b3575d7e77ff3f5";
            public const string ArmorPlus2 = "758b77a97640fd747abf149f5bf538d0";
            public const string ShieldPlus2 = "7b9f2f78a83577d49927c78be0f7fbc1";

            // +3
            public const string WeaponPlus3 = "80bb8a737579e35498177e1e3c75899b";
            public const string ArmorPlus3 = "9448d3026111d6d49b31fc85e7f3745a";
            public const string ShieldPlus3 = "ac2e3a582b5faa74aab66e0a31c935a9";

            // +4
            public const string WeaponPlus4 = "783d7d496da6ac44f9511011fc5f1979";
            public const string ArmorPlus4 = "eaeb89df5be2b784c96181552414ae5a";
            public const string ShieldPlus4 = "a5d27d73859bd19469a6dde3b49750ff";

            // +5
            public const string WeaponPlus5 = "bdba267e951851449af552aa9f9e3992";
            public const string ArmorPlus5 = "6628f9d77fd07b54c911cd8930c0d531";
            public const string ShieldPlus5 = "84d191a748edef84ba30c13b8ab83bd9";

            // +6
            public const string WeaponPlus6 = "0326d02d2e24d254a9ef626cc7a3850f";
            public const string ArmorPlus6 = "de15272d1f4eb7244aa3af47dbb754ef";
            public const string ShieldPlus6 = "70c26c66adb96d74baec38fc8d20c139";
        }

        internal static class ItemQuality
        {
            public const string Druchite = "e6a7a2b6f26b488783c612add1e9a8bd";
            public const string ColdIron = "e5990dc76d2a613409916071c898eee8";
            public const string Mithral = "0ae8fc9f2e255584faf4d14835224875";
            public const string Adamantine = "ab39e7d59dd12f4429ffef5dca88dc7b";
            public const string MasterWork = "b38844e2bffbac48b63036b66e735be";
            public const string Composite = "c3209eb058d471548928a200d70765e0";
        }

        // Ejemplo de otros ids que ya usas (añádelos cuando quieras):
        internal static class Loot
        {
            public const string DummyLootTable = "931f5cd963df3984ba96562ae0b206dd";
            public const string GoldItem = "f2bc0997c24e573448c6c91d2be88afa";
        }

        internal static class Chests
        {
            public const string DefaultLootChest = "1ccbdc2361534a8d99e4043b8b345e72";
        }

        internal static class FactionIds
        {
            public const string Bandits = "28460a5d00a62b742b80c90c37559644";
            public const string Mob = "0f539babafb47fe4586b719d02aff7c4";
            public const string Ooze = "24a215bb66e34153b4d648829c088ae6";
            public const string WildAnimals = "b1525b4b33efe0241b4cbf28486cd2cc";

            /// <summary>Fallback si no se reconoce la facción.</summary>
            public const string Unknown = "UNKNOWN";
        }
    }
}