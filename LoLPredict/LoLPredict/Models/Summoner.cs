using System;
using System.Collections.Generic;
using System.Text;

namespace DataScraper.Models
{
    public class Summoner
    {
        public int profileIconId { get; set; }
        public string name { get; set; }
        public long summonerLevel { get; set; }
        public long revisionDate { get; set; }
        public long id { get; set; }
        public long accountId { get; set; }
        public long leaguePoints { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
    }
}
