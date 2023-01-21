﻿namespace Screener.Models
{
    public class TradeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal VolumeCoin { get; set; }
        public decimal VolumeUSDT { get; set; }
        public decimal DepoPercent { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTimeUTC0 { get; set; }
        public string Direction { get; set; }
    }
}