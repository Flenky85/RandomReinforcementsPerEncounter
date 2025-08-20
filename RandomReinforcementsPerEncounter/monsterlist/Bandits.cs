using RandomReinforcementsPerEncounter.Domain.Models;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    public static class BanditList
    {
        public static readonly List<MonsterData> Monsters = new List<MonsterData>
        {
            new MonsterData
            {
                AssetId = "ae99061495c341489ac354a3bb0b8a72",  // DLC3_CR1_Marauder_Human_Conjurer_Male (Kingmaker.Localization.SharedStringAsset)
                Levels = "2",
                CR = "1",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "b779c993ec18218489e6b4b671b07073",  // BanditConjurer (Kingmaker.Localization.SharedStringAsset)
                Levels = "2",
                CR = "1",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "ca284382fc17e9740a8b1cd9ce22c75e",  // MarauderConjurer (Kingmaker.Localization.SharedStringAsset)
                Levels = "2",
                CR = "1",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "10827d5cf43a454abd922f2f0f52f268",  // CR1_Bandit_Human_FighterMelee_Male2_DLC2 (Kingmaker.Localization.SharedStringAsset)
                Levels = "2",
                CR = "1",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "46c6dbaa2dad405890a32d39d4c9b76b",  // CR1_Bandit_Human_FighterRanged_Male_DLC2 (Kingmaker.Localization.SharedStringAsset)
                Levels = "2",
                CR = "1",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "08274971c5c143cc9dc0de9528d4443d",  // CR1_GraveRobber_KnifeMasterRE_DLC2 (Kingmaker.Localization.SharedStringAsset)
                Levels = "2",
                CR = "1",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "7ed3a09e865f4514af8599671c75e11f",  // CR1_Bandit_Human_Conjurer_Male_DLC2 (Kingmaker.Localization.SharedStringAsset)
                Levels = "4",
                CR = "1",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "24088cc8784b8a0429316452bbb233b5",  // BanditConjurer (Kingmaker.Localization.SharedStringAsset)
                Levels = "2",
                CR = "2",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "0928f0c31a0b4b68b53ac73c4803c26e",  // BanditMelee (Kingmaker.Localization.SharedStringAsset)
                Levels = "2",
                CR = "2",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "39a2773ff23e4c46ab04fbf1540b8d7e",  // BanditAlchemist (Kingmaker.Localization.SharedStringAsset)
                Levels = "3",
                CR = "2",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "0a61543600fd4f85b31127ac5be4095f",  // BanditConjurer (Kingmaker.Localization.SharedStringAsset)
                Levels = "4",
                CR = "3",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "a152c1c27df0445b9e082324dfe118e8",  // CR3_Bandit_Human_Alchemist_MaleRQ (Kingmaker.Localization.SharedStringAsset)
                Levels = "4",
                CR = "3",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "5e301886958d9564badcf5a1090e0ba8",  // MarauderMelee (Kingmaker.Localization.SharedStringAsset)
                Levels = "5",
                CR = "4",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "2c21589df6e1711449a65484164ac4b9",  // BanditConjurer (Kingmaker.Localization.SharedStringAsset)
                Levels = "2",
                CR = "4",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "7c656450d3544ab18143e5967c3ede53",  // MarauderMelee (Kingmaker.Localization.SharedStringAsset)
                Levels = "5",
                CR = "4",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "846a45f126474464a701bcda059a6e9e",  // CR5_BanditFighterRangedLevel6 (Kingmaker.Localization.SharedStringAsset)
                Levels = "5",
                CR = "4",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "6359805208614003a98842e1323e52a1",  // MarauderFighter (Kingmaker.Localization.SharedStringAsset)
                Levels = "5",
                CR = "4",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "02a0fb4777b844628eb0088fe853e3d8",  // MarauderFighter (Kingmaker.Localization.SharedStringAsset)
                Levels = "5",
                CR = "4",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "4b77ec88ccd945fbb6b22f6e71698939",  // DLC3_CR5_Marauder_HalfOrc_Barbarian_Female1 (Kingmaker.Localization.SharedStringAsset)
                Levels = "1",
                CR = "5",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "e8525830047840d491ed7fadf871cbca",  // MarauderMelee (Kingmaker.Localization.SharedStringAsset)
                Levels = "1",
                CR = "5",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "9b4e80440815492c8ef511c55b59cca9",  // CR4_Goblin_RogueMeleeCaster (Kingmaker.Localization.SharedStringAsset)
                Levels = "6",
                CR = "5",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "4acf1591a57bf274da927cc475db87b2",  // MarauderAlchemist (Kingmaker.Localization.SharedStringAsset)
                Levels = "6",
                CR = "5",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "cd482d37a388609408f5dd5bd019396d",  // MarauderMelee (Kingmaker.Localization.SharedStringAsset)
                Levels = "1",
                CR = "5",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "8d7e78d529f3e9c4f8154e7f814ad81b",  // MarauderMelee (Kingmaker.Localization.SharedStringAsset)
                Levels = "1",
                CR = "5",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "81ff21d9e0fac9840aca9f6fc1a6e3f5",  // BanditConjurer (Kingmaker.Localization.SharedStringAsset)
                Levels = "2",
                CR = "6",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "4f03f48691f84c5d9e641c498d3125db",  // BanditNecromancer (Kingmaker.Localization.SharedStringAsset)
                Levels = "8",
                CR = "8",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },

            new MonsterData
            {
                AssetId = "ad4f4735eab16be44a835da799d24faf",  // CR9_BanditFighterMelee (Kingmaker.Localization.SharedStringAsset)
                Levels = "10",
                CR = "9",
                Faction = "28460a5d00a62b742b80c90c37559644"
            },
        };
    }
}