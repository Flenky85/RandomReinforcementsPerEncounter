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

        internal static class EnchantsPrefabs
        {
            public const string CrimsonMoon = "907d3c215c9522d4e8a3b763f1b32935"; // (blood)
            public const string Unholy = "e098bc50ec458af4cb7c2d828db0ce18";
            public const string Corrosive = "bfafef74d59950242915a8e294e6fac0";
            public const string Flaming = "91e5a56dd421a2941984a36a2af164b6";
            public const string Frost = "e9930f40a35b67c418e78a98c601c93b";
            public const string Shock = "1d1465ffa2699644ba8dfac48cb33195";
            public const string Vicious = "d01d77862c68852449895718902c8599"; // (Thorns)
            public const string Holy = "d739a9e236ba6164ab854b356bfb6ed5";
            public const string Sonic = "d31b9df8d99674742a161eb3faa07f3f";
            public const string Speed = "eee18332ea407bb4ea9bf2aa5f9ddf90"; // (wind green)
            public const string BrilliantEnergy = "fdc7f8f37d3f8da42be2a1d35a617001"; // (magic blue)
            public const string Axiomatic = "9c950f7e0624df24ca74d9b01a3a2cfa"; // (yellow circles)
            public const string Agile = "a4eba3360cc5b5d4ba2fe1036ce0cc8c"; // (wind blue)
            public const string Ghost = "d7b9bfb16264e4d4aad2abef2c80f835";
            public const string Anarchic = "57315bc1e1f62a741be0efde688087e9"; // (oxido)
        }
    }
}