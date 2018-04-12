using System;
using System.Collections.Generic;
using System.Text;

namespace DataScraper.Models
{
    public class MatchReference
    {
        public string lane { get; set; }
        public long gameId { get; set; }
        public int champion { get; set; }
        public int platformId { get; set; }
        public int season { get; set; }
        public int queue { get; set; }
        public string role { get; set; }
        public string timestamp { get; set; }
    }
}
