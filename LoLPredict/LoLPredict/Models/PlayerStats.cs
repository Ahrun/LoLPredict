using System;
using System.Collections.Generic;
using System.Text;

namespace DataScraper.Models
{
    public class PlayerStats
    {
        public long summonerId { get; set; }
        public string playerName { get; set; }
        public int leaguePoints { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
    }
}
