using System;
using System.Collections.Generic;
using System.Text;

namespace DataScraper.Models
{
    public class League
    {
        public string leagueId { get; set; }
        public string tier { get; set; }
        public List<LeagueItem> entries { get; set; }
        public string queue { get; set; }
        public string name { get; set; }
    }
}
