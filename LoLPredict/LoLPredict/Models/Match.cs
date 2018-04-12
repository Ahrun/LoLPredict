using System;
using System.Collections.Generic;
using System.Text;

namespace DataScraper.Models
{
    public class Match
    {
        public int seasonId { get; set; }
        public int queueId { get; set; }
        public long gameId { get; set; }
        public List<ParticipantIdentity> participantIdentities;
        public string gameVersion { get; set; }
        public string platformId { get; set; }
        public string gameMode { get; set; }
        public int mapId { get; set; }
        public string gameType { get; set; }
        public List<TeamStats> teams { get; set; }
        public List<Participant> participants { get; set; }
        public long gameDuration { get; set; }
        public long gameCreation { get; set; }
    }
}
