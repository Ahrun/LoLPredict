using System;
using System.Collections.Generic;
using System.Text;

namespace DataScraper.Models
{
    public class PlayerChampionStats
    {
        public long summonerId { get; set; }
        public int championId { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public float kda { get; set; }
        public float csPerMinute { get; set; }
        public float wardsPlacedPerMinute { get; set; }
        public float wardsKilledPerMinute { get; set; }
    }
}
