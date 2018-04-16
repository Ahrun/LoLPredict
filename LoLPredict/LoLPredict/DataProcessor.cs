using DataScraper.Models;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
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

        public List<Object> BuildPlayerStats(long summonerId, string playerName, long leaguePoints, List<Match> matches)
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
            float totalKills = 0, totalDeaths = 0, totalAssists = 0;
            float totalCs = 0, totalWardsPlaced = 0, totalWardsKilled = 0;
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
                kda = (totalKills + totalAssists) / (totalDeaths),
                wardsPlacedPerMinute = totalWardsPlaced / (totalGameTime / 60),
                wardsKilledPerMinute = totalWardsKilled / (totalGameTime / 60),
                summonerId = summonerId,
                wins = wins,
                losses = losses
            };
            return playerChampionStats;
        }

        public void GenerateNeutralNetInputs(List<MatchModel> matches, List<PlayerStats> playerStats)
        {
            List<NeuralNetInput> neuralNetInputs = new List<NeuralNetInput>();
            List<int> neuralNetOutputs = new List<int>();
            foreach(MatchModel matchModel in matches)
            {
                NeuralNetInput input = new NeuralNetInput();
                input.champions = new Dictionary<int, int>();
                input.bans = new Dictionary<int, int>();

                foreach(PlayerStats playerStat in playerStats)
                {
                    if (playerStat.summonerId == matchModel.summoner1)
                    {
                        input.summoner1wl = ((float)playerStat.wins) / ((float)playerStat.losses);
                        input.summoner1lp = playerStat.leaguePoints;
                    }
                    if (playerStat.summonerId == matchModel.summoner2)
                    {
                        input.summoner2wl = ((float)playerStat.wins) / ((float)playerStat.losses);
                        input.summoner2lp = playerStat.leaguePoints;
                    }
                    if (playerStat.summonerId == matchModel.summoner3)
                    {
                        input.summoner3wl = ((float)playerStat.wins) / ((float)playerStat.losses);
                        input.summoner3lp = playerStat.leaguePoints;
                    }
                    if (playerStat.summonerId == matchModel.summoner4)
                    {
                        input.summoner4wl = ((float)playerStat.wins) / ((float)playerStat.losses);
                        input.summoner4lp = playerStat.leaguePoints;
                    }
                    if (playerStat.summonerId == matchModel.summoner5)
                    {
                        input.summoner5wl = ((float)playerStat.wins) / ((float)playerStat.losses);
                        input.summoner5lp = playerStat.leaguePoints;
                    }
                    if (playerStat.summonerId == matchModel.summoner6)
                    {
                        input.summoner6wl = ((float)playerStat.wins) / ((float)playerStat.losses);
                        input.summoner6lp = playerStat.leaguePoints;
                    }
                    if (playerStat.summonerId == matchModel.summoner7)
                    {
                        input.summoner7wl = ((float)playerStat.wins) / ((float)playerStat.losses);
                        input.summoner7lp = playerStat.leaguePoints;
                    }
                    if (playerStat.summonerId == matchModel.summoner8)
                    {
                        input.summoner8wl = ((float)playerStat.wins) / ((float)playerStat.losses);
                        input.summoner8lp = playerStat.leaguePoints;
                    }
                    if (playerStat.summonerId == matchModel.summoner9)
                    {
                        input.summoner9wl = ((float)playerStat.wins) / ((float)playerStat.losses);
                        input.summoner9lp = playerStat.leaguePoints;
                    }
                    if (playerStat.summonerId == matchModel.summoner10)
                    {
                        input.summoner10wl = ((float)playerStat.wins) / ((float)playerStat.losses);
                        input.summoner10lp = playerStat.leaguePoints;
                    }
                }
                if (input.summoner1wl == 0f) { input.summoner1wl = 0.5f; input.summoner1lp = 0; }
                if (input.summoner2wl == 0f) { input.summoner2wl = 0.5f; input.summoner2lp = 0; }
                if (input.summoner3wl == 0f) { input.summoner3wl = 0.5f; input.summoner3lp = 0; }
                if (input.summoner4wl == 0f) { input.summoner4wl = 0.5f; input.summoner4lp = 0; }
                if (input.summoner5wl == 0f) { input.summoner5wl = 0.5f; input.summoner5lp = 0; }
                if (input.summoner6wl == 0f) { input.summoner6wl = 0.5f; input.summoner6lp = 0; }
                if (input.summoner7wl == 0f) { input.summoner7wl = 0.5f; input.summoner7lp = 0; }
                if (input.summoner8wl == 0f) { input.summoner8wl = 0.5f; input.summoner8lp = 0; }
                if (input.summoner9wl == 0f) { input.summoner9wl = 0.5f; input.summoner9lp = 0; }
                if (input.summoner10wl == 0f) { input.summoner10wl = 0.5f; input.summoner10lp = 0; }

                foreach(Champion champ in Enum.GetValues(typeof(Champion)))
                {
                    input.champions[(int)champ] = 0;
                    input.bans[(int)champ] = 0;
                }
                input.champions[matchModel.champion1] = 1;
                input.champions[matchModel.champion2] = 1;
                input.champions[matchModel.champion3] = 1;
                input.champions[matchModel.champion4] = 1;
                input.champions[matchModel.champion5] = 1;
                input.champions[matchModel.champion6] = 1;
                input.champions[matchModel.champion7] = 1;
                input.champions[matchModel.champion8] = 1;
                input.champions[matchModel.champion9] = 1;
                input.champions[matchModel.champion10] = 1;

                input.bans[matchModel.ban1] = 1;
                input.bans[matchModel.ban2] = 1;
                input.bans[matchModel.ban3] = 1;
                input.bans[matchModel.ban4] = 1;
                input.bans[matchModel.ban5] = 1;
                input.bans[matchModel.ban6] = 1;
                input.bans[matchModel.ban7] = 1;
                input.bans[matchModel.ban8] = 1;
                input.bans[matchModel.ban9] = 1;
                input.bans[matchModel.ban10] = 1;

                neuralNetInputs.Add(input);
                neuralNetOutputs.Add(matchModel.sideWin);
            }
            File.WriteAllText("X.csv",CsvSerializer.SerializeToCsv<NeuralNetInput>(neuralNetInputs));
            File.WriteAllText("Y.csv", CsvSerializer.SerializeToCsv<int>(neuralNetOutputs));
        }
    }
}