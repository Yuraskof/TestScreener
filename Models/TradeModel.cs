namespace Screener.Models
{
    public class TradeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Direction { get; set; }
        public decimal VolumeUSDT { get; set; }
        public decimal VolumeCoin { get; set; }
        public decimal PositionDepoPercent { get; set; }
        public decimal AllPositionsDepoPercent { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTimeUTC0 { get; set; }
        public int OpenPositionsCount { get; set; }
        public decimal Depo { get; set; }
        public string Comment { get; set; }
        public List<PositionModel> AllOpenNowPositions { get; set; }
    }
}
