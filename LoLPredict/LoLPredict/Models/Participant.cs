using System;
using System.Collections.Generic;
using System.Text;

namespace DataScraper.Models
{
    public class Participant
    {
        public ParticipantStats stats { get; set; }
        public int participantId { get; set; }
        public List<Rune> runes { get; set; }
        public ParticipantTimeline timeline { get; set; }
        public int teamId { get; set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public List<Mastery> masteries { get; set; }
        public string highestAchievedSeasonTier { get; set; }
        public int championId { get; set; }
    }
}
