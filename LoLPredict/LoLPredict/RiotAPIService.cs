using System;
using System.Net.Http;

namespace DataScraper
{
    public class RiotAPIService
    {
        int limiter = 0;
        private static HttpClient _httpClient = new HttpClient();

        private string challengerLeagues = "/lol/league/v3/challengerleagues/by-queue/{queue}";
        private string masterLeagues = "/lol/league/v3/masterleagues/by-queue/{queue}";

        private string matchListEndpoint = "/lol/match/v3/matchlists/by-account/{accountId}";
        private string recentMatchListEndpoint = "/lol/match/v3/matchlists/by-account/{accountId}/recent";
        private string matchEndpoint = "/lol/match/v3/matches/{matchId}";

        private string summonerEndpoint = "/lol/summoner/v3/summoners/{summonerId}";
        public RiotAPIService(string apiKey)
        {
            _httpClient.BaseAddress = new Uri("https://na1.api.riotgames.com");
            _httpClient.DefaultRequestHeaders.Add("api_key", apiKey);
        }
        //TODO: Implement API calls with expected optional parameters
        //Build models for return types of all methods
        //Write to SQLite Database for data storage
        //Determine size of dataset needed

        private void RateLimiter()
        {
            if (limiter > 50)
            {
                System.Threading.Thread.Sleep(300000);
                limiter = 0;
            }
            limiter++;
        }
    }
}
