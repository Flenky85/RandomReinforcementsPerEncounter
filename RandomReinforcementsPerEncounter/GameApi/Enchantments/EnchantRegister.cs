using RandomReinforcementsPerEncounter.Domain.Models;                 // EnchantTierConfig
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Builder;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments
{
    internal static class EnchantRegister
    {
        private static bool _done;

        internal static void RegisterAll()
        {
            if (_done) return; // idempotente
            _done = true;

            // 1) Preparar loot
            LootBuckets.Clear();
            LootBuckets.SetDefaultValue(1);

            // 2) Construir blueprints
            var enchantList = new[]
            {
                // OnHit
                EnchantCatalog.Fearsome,
                EnchantCatalog.Blinding,
                EnchantCatalog.Dazzling,
                EnchantCatalog.Sickening,
                EnchantCatalog.Staggering,
                EnchantCatalog.Fatiguing,
                EnchantCatalog.Confusing,
                EnchantCatalog.Entangling,
                EnchantCatalog.Slowing,

                // OnlyOnFirstHit
                EnchantCatalog.Frightening,
                EnchantCatalog.Stunning,
                EnchantCatalog.Dazing,
                EnchantCatalog.Slumbering,
                EnchantCatalog.Paralyzing,
                EnchantCatalog.Exhausting,
                EnchantCatalog.Nauseating,
                EnchantCatalog.Toppling,
                EnchantCatalog.Dominating,

                // EnergyDamage
                EnchantCatalog.Flaming,
                EnchantCatalog.Frost,
                EnchantCatalog.Shock,
                EnchantCatalog.Thundering,
                EnchantCatalog.Corrosive,
                EnchantCatalog.Unholy,
                EnchantCatalog.Holy,

                // StatsBonus
                EnchantCatalog.StatSTR,
                EnchantCatalog.StatDEX,
                EnchantCatalog.StatCON,
                EnchantCatalog.StatINT,
                EnchantCatalog.StatWIS,
                EnchantCatalog.StatCHA,

                //SkillBonus
                EnchantCatalog.SkillMOB,
                EnchantCatalog.SkillATH,
                EnchantCatalog.SkillARC,
                EnchantCatalog.SkillWOR,
                EnchantCatalog.SkillNAT,
                EnchantCatalog.SkillREL,
                EnchantCatalog.SkillPERC,
                EnchantCatalog.SkillPERS,
                EnchantCatalog.SkillSTE,
                EnchantCatalog.SkillTHI,
                EnchantCatalog.SkillUMD,

                //Other
                EnchantCatalog.CMB,
                EnchantCatalog.CMD,
                EnchantCatalog.SaveFOR,
                EnchantCatalog.SaveWIL,
                EnchantCatalog.SaveREF,
                EnchantCatalog.Initiative,

                // Caster Level
                EnchantCatalog.CasterLevel,

                // Caster (features)
                EnchantCatalog.SpellDC,
                EnchantCatalog.SpellDieBonus,

                // School CL (features)
                EnchantCatalog.DivinationCL,
                EnchantCatalog.EnchantmentCL,
                EnchantCatalog.EvocationCL,
                EnchantCatalog.ConjurationCL,
                EnchantCatalog.AbjurationCL,
                EnchantCatalog.IllusionCL,
                EnchantCatalog.TransmutationCL,
                EnchantCatalog.NecromancyCL,

                // School DC (features)
                EnchantCatalog.DivinationDC,
                EnchantCatalog.EnchantmentDC,
                EnchantCatalog.EvocationDC,
                EnchantCatalog.ConjurationDC,
                EnchantCatalog.AbjurationDC,
                EnchantCatalog.IllusionDC,
                EnchantCatalog.TransmutationDC,
                EnchantCatalog.NecromancyDC,
            };

            foreach (var def in enchantList)
                EnchantBuilder.Build(def);

            // --- Price tiers T1..T6: 200 → 6400 (doblando)
            var tiers = new List<EnchantTierConfig>(6);
            int delta = 200;
            for (int i = 0; i < 6; i++)
            {
                tiers.Add(new EnchantTierConfig
                {
                    AssetId = GuidUtil.EnchantGuid($"price_{delta}").ToString()
                    // si prefieres helper: AssetId = GuidUtil.EnchantId($"price_{delta}")
                });
                delta <<= 1; 
            }

            EnchantFactory.RegisterWeaponPriceForTiers(tiers, baseDelta: 200);

            // 3) Volcar a la lista final
            LootBuckets.FlushToEnchantList(EnchantList.Item);
        }
    }
}
