using DataScraper;
using DataScraper.Models;
using System.Collections.Generic;

namespace LoLPredict
{
    public class Program
    {

        static void Main(string[] args)
        {
            string apiKey = "Fake LoL API key";
            RiotAPIService riotApi = new RiotAPIService(apiKey);
            DatabaseAccess databaseAccess = new DatabaseAccess();
            DataProcessor dataProcessor = new DataProcessor();

            /*
             * 1. Get all Challenger players
             * 2. Get all recent games to build list of matches
             * 3. For each player in those matches, acquire all ranked 5x5 games from season 8 for them 
             * 4. From each of those games (written to database after each summoner so they can be read from database rather than gotten), build player season stats
             * 5. Get any other relevant summoner info (rank, LP, hot streak, etc.)
             * 6. Create AI project to preprocess data into usable features
             * 7. Create neural net/other classification models within AI project to utilize acquired data.
             * 
             */
            //List<Summoner> challengerPlayers = (List<Summoner>) riotApi.GetChallengerLeague().Result;
            //List<Summoner> masterPlayers = (List<Summoner>)riotApi.GetMasterLeague().Result;
            //challengerPlayers.AddRange(masterPlayers);
            //var recentGames = new List<Match>();
            //var matchList = new List<Match>();
            //foreach (Summoner challengerPlayer in challengerPlayers)
            //{
            //    var recentGamesToAdd = riotApi.GetRecentMatchList(challengerPlayer.accountId).Result;
            //    recentGames.AddRange(recentGamesToAdd);
            //    var playerStats = new PlayerStats()
            //    {
            //        leaguePoints = challengerPlayer.leaguePoints,
            //        losses = challengerPlayer.losses,
            //        playerName = challengerPlayer.name,
            //        summonerId = challengerPlayer.id,
            //        wins = challengerPlayer.wins
            //    };
            //    databaseAccess.WritePlayerStats(playerStats);
            //    //var matchesToAdd = riotApi.GetMatchList(challengerPlayer.accountId).Result;
            //    //matchList.AddRange(matchesToAdd);
            //    //var stats = dataProcessor.BuildPlayerStats(challengerPlayer.id, challengerPlayer.name, challengerPlayer.leaguePoints, (List<Match>)matchesToAdd);

            //    /*foreach (PlayerChampionStats playerchampStats in (List<PlayerChampionStats>)stats[1])
            //    {
            //        databaseAccess.WritePlayerChampionStats(playerchampStats);
            //    }*/
            //}
            //foreach (Match match in recentGames)
            //{   if (match != null && match.queueId == 420 && match.seasonId == 11)
            //    {
            //        databaseAccess.WriteMatch(dataProcessor.BuildMatchModel(match));
            //    }
            //}
            List<MatchModel> matches = databaseAccess.ReadMatches();
            List <PlayerStats> players = databaseAccess.ReadPlayerStats();

            dataProcessor.GenerateNeutralNetInputs(matches, players);
        }
    }
}
