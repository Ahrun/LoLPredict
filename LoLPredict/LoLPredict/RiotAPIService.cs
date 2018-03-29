using RiotSharp;
using RiotSharp.LeagueEndpoint;
using RiotSharp.MatchEndpoint;
using System;
using System.Collections.Generic;

namespace DataScraper
{
    public class RiotAPIService
    {
        RiotApi _riotApi;
        int limiter = 0;
        public RiotAPIService(string apiKey)
        {
            _riotApi = RiotApi.GetDevelopmentInstance(apiKey);
        }

        public List<LeaguePosition> GetChallengerLeague()
        {
            RateLimiter();
            var challenger = _riotApi.GetChallengerLeague(RiotSharp.Misc.Region.na, "RANKED_SOLO_5x5");
            return challenger.Entries;
        }

        public List<LeaguePosition> GetMasterLeague()
        {
            RateLimiter();
            var master = _riotApi.GetMasterLeague(RiotSharp.Misc.Region.na, "RANKED_SOLO_5x5");
            return master.Entries;
        }

        public List<MatchReference> GetRecentMatches(LeaguePosition leaguePosition)
        {
            RateLimiter();
            var accountId = _riotApi.GetSummonerBySummonerId(RiotSharp.Misc.Region.na, Int64.Parse(leaguePosition.PlayerOrTeamId)).AccountId;
            RateLimiter();
            return _riotApi.GetRecentMatches(RiotSharp.Misc.Region.na, accountId);
        }

        public Match GetMatch(long matchId)
        {
            RateLimiter();
            return _riotApi.GetMatch(RiotSharp.Misc.Region.na, matchId);
        }

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
