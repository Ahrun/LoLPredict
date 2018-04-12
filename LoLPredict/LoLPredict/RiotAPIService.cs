using DataScraper.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Riot-Token", apiKey);
        }
        //TODO: Implement API calls with expected optional parameters
        //Build models for return types of all methods
        //Write to SQLite Database for data storage
        //Determine size of dataset needed
        public async Task<IEnumerable<LeaguePlayer>> getChallengerLeague()
        {
            try
            {
                RateLimiter();
                HttpResponseMessage response = await _httpClient.GetAsync(challengerLeagues.Replace("{queue}", "RANKED_SOLO_5x5"));
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiException();
                }
                League challenger = JsonConvert.DeserializeObject<League>(await response.Content.ReadAsStringAsync());
                return challenger.entries;
            }
            catch(ApiException ex)
            {
                if(retryCount < retryCountLimit)
                {
                    Retry();
                }
                else
                {
                    return new List<LeaguePlayer>();
                }
            }
        }

        public async Task<IEnumerable<Match>> GetMatchList(int accountId)
        {
            int beginIndex = 0;
            List<Match> matchList = new List<Match>();
            List<Match> matchBatch;
            do
            {
                matchBatch = new List<Match>();
                string queryParamedUri = matchListEndpoint;
                queryParamedUri = queryParamedUri.Replace("{accountId}", "" + accountId);
                queryParamedUri += "?queue=420&season=11&beginIndex=" + beginIndex;
                RateLimiter();
                HttpResponseMessage response = await _httpClient.GetAsync(queryParamedUri);
                var matchListResponse = JsonConvert.DeserializeObject<MatchList>(await response.Content.ReadAsStringAsync());
                matchBatch.AddRange(matchListResponse.matches);
                matchList.AddRange(matchBatch);
                beginIndex += 100;
            } while (matchBatch.Count == 100);

            return matchList;
        }

        public async Task<IEnumerable<Match>> GetRecentMatchList(int accountId)
        {
            RateLimiter();
            HttpResponseMessage response = await _httpClient.GetAsync(recentMatchListEndpoint.Replace("{accountId}",""+accountId));
            var matchListResponse = JsonConvert.DeserializeObject<MatchList>(await response.Content.ReadAsStringAsync());
            return matchListResponse.matches;
        }

        public async Task<Match> GetMatch(int gameId)
        {
            RateLimiter();
            HttpResponseMessage response = await _httpClient.GetAsync(matchEndpoint.Replace("{matchId}", "" + gameId));
            return JsonConvert.DeserializeObject<Match>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Summoner> GetSummoner(int summonerId)
        {
            RateLimiter();
            HttpResponseMessage response = await _httpClient.GetAsync(summonerEndpoint.Replace("{summonerId}", "" + summonerId));
            return JsonConvert.DeserializeObject<Summoner>(await response.Content.ReadAsStringAsync());
        }
        private void RateLimiter()
        {
            if (limiter > 10)
            {
                System.Threading.Thread.Sleep(60000);
                limiter = 0;
            }
            limiter++;
        }
    }
}
