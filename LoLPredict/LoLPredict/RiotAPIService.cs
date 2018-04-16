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
        public async Task<IEnumerable<Summoner>> GetChallengerLeague()
        {
            List<Summoner> summoners = new List<Summoner>();
            RateLimiter();
            HttpResponseMessage response = await _httpClient.GetAsync(challengerLeagues.Replace("{queue}", "RANKED_SOLO_5x5"));
            League challenger = JsonConvert.DeserializeObject<League>(await response.Content.ReadAsStringAsync());
            foreach(LeagueItem leagueItem in challenger.entries)
            {
                Summoner toAdd = await GetSummoner(int.Parse(leagueItem.playerOrTeamId));
                toAdd.leaguePoints = leagueItem.leaguePoints;
                toAdd.losses = leagueItem.losses;
                toAdd.wins = leagueItem.wins;
                summoners.Add(toAdd);
            }
            return summoners;
        }

        public async Task<IEnumerable<Summoner>> GetMasterLeague()
        {
            List<Summoner> summoners = new List<Summoner>();
            RateLimiter();
            HttpResponseMessage response = await _httpClient.GetAsync(masterLeagues.Replace("{queue}", "RANKED_SOLO_5x5"));
            League challenger = JsonConvert.DeserializeObject<League>(await response.Content.ReadAsStringAsync());
            foreach (LeagueItem leagueItem in challenger.entries)
            {
                Summoner toAdd = await GetSummoner(int.Parse(leagueItem.playerOrTeamId));
                toAdd.leaguePoints = leagueItem.leaguePoints;
                toAdd.losses = leagueItem.losses;
                toAdd.wins = leagueItem.wins;
                summoners.Add(toAdd);
            }
            return summoners;
        }

        public async Task<IEnumerable<Match>> GetMatchList(long accountId)
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
                foreach(MatchReference matchReference in matchListResponse.matches){
                    matchBatch.Add(await GetMatch(matchReference.gameId));
                }
                matchList.AddRange(matchBatch);
                beginIndex += 100;
            } while (matchBatch.Count == 100);

            return matchList;
        }

        public async Task<IEnumerable<Match>> GetRecentMatchList(long accountId)
        {
            List<Match> matchList = new List<Match>();
            RateLimiter();
            HttpResponseMessage response = await _httpClient.GetAsync(recentMatchListEndpoint.Replace("{accountId}",""+accountId));
            var matchListResponse = JsonConvert.DeserializeObject<MatchList>(await response.Content.ReadAsStringAsync());
            foreach (MatchReference matchReference in matchListResponse.matches)
            {
                if (matchReference != null && matchReference.queue == 420 && matchReference.season == 11)
                {
                    matchList.Add(await GetMatch(matchReference.gameId));
                }
            }
            return matchList;
        }

        public async Task<Match> GetMatch(long gameId)
        {
            RateLimiter();
            HttpResponseMessage response = await _httpClient.GetAsync(matchEndpoint.Replace("{matchId}", "" + gameId));
            return JsonConvert.DeserializeObject<Match>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Summoner> GetSummoner(long summonerId)
        {
            RateLimiter();
            HttpResponseMessage response = await _httpClient.GetAsync(summonerEndpoint.Replace("{summonerId}", "" + summonerId));
            return JsonConvert.DeserializeObject<Summoner>(await response.Content.ReadAsStringAsync());
        }
        private void RateLimiter()
        {
            if(limiter % 5 == 0)
            {
                System.Threading.Thread.Sleep(1000);
            }
            if (limiter > 95)
            {
                System.Threading.Thread.Sleep(120000);
                limiter = 0;
            }
            limiter++;
        }
    }
}
