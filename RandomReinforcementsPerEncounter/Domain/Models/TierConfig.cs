namespace RandomReinforcementsPerEncounter.Domain.Models
{
    public class EnchantTierConfig
    {
        public string AssetId { get; set; }
        public int DC { get; set; }           // usado por debuffs (salvación)
        public int DiceCount { get; set; }    // usado por daño de energía
        public int DiceSide { get; set; }     // usado por daño de energía
        public int Bonus { get; set; }        // usado por bonificadores a stats
        public int BonusDescription { get; set; } // para textos de bonus “+X”
        public string Feat { get; set; }      // feature a otorgar con el enchant
    }

    public class FeatureTierConfig
    {
        public string AssetId { get; set; }
        public int Bonus { get; set; }
    }
}