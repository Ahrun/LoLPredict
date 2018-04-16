using System;
using System.Collections.Generic;
using System.Text;

namespace DataScraper.Models
{
    public class NeuralNetInput
    {
        public Dictionary<int, int> champions { get; set; }
        public Dictionary<int, int> bans { get; set; }
        public float summoner1wl { get; set; }
        public float summoner2wl { get; set; }
        public float summoner3wl { get; set; }
        public float summoner4wl { get; set; }
        public float summoner5wl { get; set; }
        public float summoner6wl { get; set; }
        public float summoner7wl { get; set; }
        public float summoner8wl { get; set; }
        public float summoner9wl { get; set; }
        public float summoner10wl { get; set; }
        public long summoner1lp { get; set; }
        public long summoner2lp { get; set; }
        public long summoner3lp { get; set; }
        public long summoner4lp { get; set; }
        public long summoner5lp { get; set; }
        public long summoner6lp { get; set; }
        public long summoner7lp { get; set; }
        public long summoner8lp { get; set; }
        public long summoner9lp { get; set; }
        public long summoner10lp { get; set; }
    }
}