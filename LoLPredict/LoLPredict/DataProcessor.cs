using DataScraper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataScraper
{
    public class DataProcessor
    {
        public MatchModel BuildMatchModel(Match match)
        {
            MatchModel matchModel = new MatchModel()
            {
                matchId = match.gameId,
                summoner1 = match.participantIdentities[0].player.summonerId,
                summoner2 = match.participantIdentities[1].player.summonerId,
                summoner3 = match.participantIdentities[2].player.summonerId,
                summoner4 = match.participantIdentities[3].player.summonerId,
                summoner5 = match.participantIdentities[4].player.summonerId,
                summoner6 = match.participantIdentities[5].player.summonerId,
                summoner7 = match.participantIdentities[6].player.summonerId,
                summoner8 = match.participantIdentities[7].player.summonerId,
                summoner9 = match.participantIdentities[8].player.summonerId,
                summoner10 = match.participantIdentities[9].player.summonerId,
                champion1 = match.participants[0].championId,
                champion2 = match.participants[1].championId,
                champion3 = match.participants[2].championId,
                champion4 = match.participants[3].championId,
                champion5 = match.participants[4].championId,
                champion6 = match.participants[5].championId,
                champion7 = match.participants[6].championId,
                champion8 = match.participants[7].championId,
                champion9 = match.participants[8].championId,
                champion10 = match.participants[9].championId,
                ban1 = match.teams[0].bans[0].championId,
                ban2 = match.teams[0].bans[1].championId,
                ban3 = match.teams[0].bans[2].championId,
                ban4 = match.teams[0].bans[3].championId,
                ban5 = match.teams[0].bans[4].championId,
                ban6 = match.teams[1].bans[0].championId,
                ban7 = match.teams[1].bans[1].championId,
                ban8 = match.teams[1].bans[2].championId,
                ban9 = match.teams[1].bans[3].championId,
                ban10 = match.teams[1].bans[4].championId,
                sideWin = match.teams[0].win == "Win" ? 0 : 1
            };
            return matchModel;
        }

        public List<Object> BuildPlayerStats(long summonerId, string playerName, int leaguePoints, List<Match> matches)
        {
            Dictionary<int, List<Match>> playerChampionStats = new Dictionary<int, List<Match>>();
            int wins = 0, losses = 0;
            foreach(Match match in matches)
            {
                int participantId = 0;
                foreach(ParticipantIdentity participantIdentity in match.participantIdentities)
                {
                    if(participantIdentity.player.summonerId == summonerId)
                    {
                        participantId = participantIdentity.ParticipantId;
                        break;
                    }
                }

                int championId = match.participants[participantId - 1].championId;
                if (playerChampionStats.ContainsKey(championId))
                {
                    playerChampionStats[championId].Add(match);
                }
                else
                {
                    playerChampionStats.Add(championId, new List<Match>
                    {
                        match
                    });
                }

                if((participantId < 6 && match.teams[0].win == "Win") || (participantId >= 6 && match.teams[0].win == "Fail"))
                {
                    wins++;
                }
                else
                {
                    losses++;
                }
            }
            List<PlayerChampionStats> playerChampionStatsList = new List<PlayerChampionStats>();
            foreach(KeyValuePair<int, List<Match>> kvp in playerChampionStats)
            {
                playerChampionStatsList.Add(BuildPlayerChampionStats(summonerId, kvp.Key, kvp.Value));
            }
            PlayerStats playerStats = new PlayerStats()
            {
                summonerId = summonerId,
                playerName = playerName,
                leaguePoints = leaguePoints,
                wins = wins,
                losses = losses
            };
            return new List<Object> { playerStats, playerChampionStatsList };
        }

        public PlayerChampionStats BuildPlayerChampionStats(long summonerId, int championId, List<Match> matches)
        {
            int wins = 0, losses = 0;
            int totalKills = 0, totalDeaths = 0, totalAssists = 0;
            int totalCs = 0, totalWardsPlaced = 0, totalWardsKilled = 0;
            long totalGameTime = 0;
            foreach(Match match in matches)
            {
                int participantId = 0;
                foreach (ParticipantIdentity participantIdentity in match.participantIdentities)
                {
                    if (participantIdentity.player.summonerId == summonerId)
                    {
                        participantId = participantIdentity.ParticipantId;
                        break;
                    }
                }
                totalKills += match.participants[participantId - 1].stats.kills;
                totalDeaths += match.participants[participantId - 1].stats.deaths;
                totalAssists += match.participants[participantId - 1].stats.assists;
                totalCs += match.participants[participantId - 1].stats.neutralMinionsKilled + match.participants[participantId - 1].stats.totalMinionsKilled;
                totalWardsPlaced = match.participants[participantId - 1].stats.wardsPlaced;
                totalWardsKilled = match.participants[participantId - 1].stats.wardsKilled;
                if ((participantId < 6 && match.teams[0].win == "Win") || (participantId >= 6 && match.teams[0].win == "Fail"))
                {
                    wins++;
                }
                else
                {
                    losses++;
                }
                totalGameTime += match.gameDuration;
            }
            PlayerChampionStats playerChampionStats = new PlayerChampionStats()
            {
                championId = championId,
                csPerMinute = totalCs / (totalGameTime / 60),
                kda = (totalKills + totalAssists) / (.0f + totalDeaths),
                wardsPlacedPerMinute = totalWardsPlaced / (totalGameTime / 60),
                wardsKilledPerMinute = totalWardsKilled / (totalGameTime / 60),
                summonerId = summonerId,
                wins = wins,
                losses = losses
            };
            return playerChampionStats;
        }
    }
}