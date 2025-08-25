namespace RandomReinforcementsPerEncounter.Domain.Models
{
    public class EnchantTierConfig
    {
        public string AssetId { get; set; }
        public int DC { get; set; }           
        public int DiceCount { get; set; }    
        public int DiceSide { get; set; }     
        public int Bonus { get; set; }        
        public int BonusDescription { get; set; }
        public string Feat { get; set; }      
    }

    public class FeatureTierConfig
    {
        public string AssetId { get; set; }
        public int Bonus { get; set; }
    }
}