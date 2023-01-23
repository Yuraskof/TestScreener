using Screener.Utilities;

namespace Screener.Models
{
    public class PositionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Direction { get; set; }
        public decimal VolumeUSDT { get; set; }
        public decimal VolumeCoin { get; set; }
        public decimal EntryPrice { get; set; }
        public string Leverage { get; set; }
        public decimal PnL { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            PositionModel other = (PositionModel)obj;

            if (VolumeCoin.Equals(other.VolumeCoin) &&
                VolumeUSDT.Equals(other.VolumeUSDT) && 
                EntryPrice.Equals(other.EntryPrice))
            {
                LoggerUtils.LogStep(nameof(Equals) + " 'Test models are equal'");
                return true;
            }
            LoggerUtils.LogStep(nameof(Equals) + " 'Test models are not equal'");
            return false;
        }
    }
}
