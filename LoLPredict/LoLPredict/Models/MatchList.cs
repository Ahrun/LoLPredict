using System;
using System.Collections.Generic;
using System.Text;

namespace DataScraper.Models
{
    public class MatchList
    {
        public List<MatchReference> matches;
        public int totalGames;
        public int startIndex;
        public int endIndex;
    }
}
