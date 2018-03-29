using DataScraper;
using RiotSharp.MatchEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoLPredict
{
    public class Program
    {

        static void Main(string[] args)
        {
            RiotAPIService riotApi = new RiotAPIService("RGAPI-d083dda5-f726-46e8-9b79-315652a446c0");
            var challengerPlayers = riotApi.GetChallengerLeague();
            var masterPlayers = riotApi.GetMasterLeague();
            List<long> matchIds = new List<long>();

            foreach (var challengerPlayer in challengerPlayers)
            {
                var playerMatches = riotApi.GetRecentMatches(challengerPlayer);
                foreach(var playerMatch in playerMatches)
                {
                    matchIds.Add(playerMatch.GameId);
                }
            }
            foreach(var masterPlayer in masterPlayers)
            {
                var playerMatches = riotApi.GetRecentMatches(masterPlayer);
                foreach (var playerMatch in playerMatches)
                {
                    matchIds.Add(playerMatch.GameId);
                }
            }
            matchIds = matchIds.Distinct().ToList();
            List<Match> matches = new List<Match>();
            foreach(long matchId in matchIds){
                matches.Add(riotApi.GetMatch(matchId));
            }
        }
    }
}
