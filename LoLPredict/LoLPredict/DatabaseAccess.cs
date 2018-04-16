using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using DataScraper.Models;

namespace DataScraper
{
    public class DatabaseAccess
    {
        SQLiteConnection _sqlConnection = new SQLiteConnection();

        public DatabaseAccess()
        {
            _sqlConnection.ConnectionString = "Data source=Database.sqlite";
            if (!File.Exists("Database.sqlite"))
            {
                CreateDatabase();
            }
        }

        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile("Database.sqlite");
            _sqlConnection.ConnectionString = "Data source=Database.sqlite";
            _sqlConnection.Open();
            CreateTables();
            _sqlConnection.Close();
        }

        private void CreateTables()
        {
            SQLiteCommand command;
            string createMatchTableSQL = "CREATE TABLE match (matchId INTEGER PRIMARY KEY, summoner1 INTEGER, summoner2 INTEGER, summoner3 INTEGER, summoner4 INTEGER, summoner5 INTEGER, " +
                "summoner6 INTEGER, summoner7 INTEGER, summoner8 INTEGER, summoner9 INTEGER, summoner10 INTEGER, champion1 INTEGER, champion2 INTEGER, champion3 INTEGER, champion4 INTEGER, " +
                "champion5 INTEGER, champion6 INTEGER, champion7 INTEGER, champion8 INTEGER, champion9 INTEGER, champion10 INTEGER, sideWin INTEGER, ban1 INTEGER, ban2 INTEGER, ban3 INTEGER, " +
                "ban4 INTEGER, ban5 INTEGER, ban6 INTEGER, ban7 INTEGER, ban8 INTEGER, ban9 INTEGER, ban10 INTEGER);";
            command = new SQLiteCommand(createMatchTableSQL, _sqlConnection);
            command.ExecuteNonQuery();
            string createPlayerStatsTableSQL = "CREATE TABLE playerStats(summonerId INTEGER PRIMARY KEY, playerName varchar(32), leaguePoints INTEGER, wins INTEGER, losses INTEGER);";
            command = new SQLiteCommand(createPlayerStatsTableSQL, _sqlConnection);
            command.ExecuteNonQuery();
            string createPlayerChampionStatsTableSQL = "CREATE TABLE playerChampionStats(summonerId INTEGER, championId INTEGER, wins INTEGER, losses INTEGER, kda FLOAT, csPerMin FLOAT, " +
                "wardsPlacedPerMinute FLOAT, wardsKilledPerMinute FLOAT, PRIMARY KEY(summonerId, championId));";
            command = new SQLiteCommand(createPlayerChampionStatsTableSQL, _sqlConnection);
            command.ExecuteNonQuery();
        }

        public void WriteMatch(MatchModel match)
        {
            _sqlConnection.Open();
            string insertMatchModelSQL = "INSERT INTO match(matchId, summoner1, summoner2, summoner3, summoner4, summoner5, summoner6, summoner7, summoner8, summoner9, summoner10, " +
                "champion1, champion2, champion3, champion4, champion5, champion6, champion7, champion8, champion9, champion10, " +
                "ban1, ban2, ban3, ban4, ban5, ban6, ban7, ban8, ban9, ban10, " +
                "sideWin) VALUES (" + match.matchId + "," + match.summoner1 + "," + match.summoner2 + "," + match.summoner3 + "," + match.summoner4 + "," + match.summoner5 + ","
                + match.summoner6 + "," + match.summoner7 + "," + match.summoner8 + "," + match.summoner9 + "," + match.summoner10 + "," + match.champion1 + "," + match.champion2 + "," + match.champion3 + "," + match.champion4 + "," + match.champion5 + ","
                + match.champion6 + "," + match.champion7 + "," + match.champion8 + "," + match.champion9 + "," + match.champion10 + "," + match.ban1 + "," + match.ban2 + "," + match.ban3 + "," + match.ban4 + "," + match.ban5 + ","
                + match.ban6 + "," + match.ban7 + "," + match.ban8 + "," + match.ban9 + "," + match.ban10 + "," + match.sideWin + ");";
            try
            {
                new SQLiteCommand(insertMatchModelSQL, _sqlConnection).ExecuteNonQuery();
            }
            catch
            {

            }
            _sqlConnection.Close();

        }

        public void WritePlayerStats(PlayerStats playerStats)
        {
            _sqlConnection.Open();
            string insertPlayerStatsSQL = "INSERT INTO playerStats(summonerId, playerName, leaguePoints, wins, losses) VALUES (" + playerStats.summonerId +
                "," + "\"" + playerStats.playerName + "\"" + "," + playerStats.leaguePoints + "," + playerStats.wins + "," + playerStats.losses + ");";
            try
            {
                new SQLiteCommand(insertPlayerStatsSQL, _sqlConnection).ExecuteNonQuery();
            }
            catch { }
            _sqlConnection.Close();
        }

        public void WritePlayerChampionStats(PlayerChampionStats playerChampionStats)
        {
            _sqlConnection.Open();
            string insertPlayerChampionStatsSQL = "INSERT INTO playerChampionStats(summonerId, championId, wins, losses, kda, csPerMin, wardsPlacedPerMinute, wardsKilledPerMinute) " +
                "VALUES (" + playerChampionStats.summonerId + "," + playerChampionStats.championId + "," + playerChampionStats.wins + "," + playerChampionStats.losses + "," 
                + playerChampionStats.kda + "," + playerChampionStats.csPerMinute + "," + playerChampionStats.wardsPlacedPerMinute + "," + playerChampionStats.wardsKilledPerMinute +");";
            try
            {
                new SQLiteCommand(insertPlayerChampionStatsSQL, _sqlConnection).ExecuteNonQuery();
            }
            catch
            {
            }
            _sqlConnection.Close();
        }

        public List<MatchModel> ReadMatches()
        {
            List<MatchModel> matches = new List<MatchModel>();
            _sqlConnection.Open();
            string selectMatchSQL = "SELECT * FROM match";
            try
            {
                SQLiteDataReader reader = new SQLiteCommand(selectMatchSQL, _sqlConnection).ExecuteReader();
                reader.Read();
                do
                {
                    matches.Add(new MatchModel
                    {
                        matchId = reader.GetInt64(0),
                        summoner1 = reader.GetInt64(1),
                        summoner2 = reader.GetInt64(2),
                        summoner3 = reader.GetInt64(3),
                        summoner4 = reader.GetInt64(4),
                        summoner5 = reader.GetInt64(5),
                        summoner6 = reader.GetInt64(6),
                        summoner7 = reader.GetInt64(7),
                        summoner8 = reader.GetInt64(8),
                        summoner9 = reader.GetInt64(9),
                        summoner10 = reader.GetInt64(10),
                        champion1 = reader.GetInt32(11),
                        champion2 = reader.GetInt32(12),
                        champion3 = reader.GetInt32(13),
                        champion4 = reader.GetInt32(14),
                        champion5 = reader.GetInt32(15),
                        champion6 = reader.GetInt32(16),
                        champion7 = reader.GetInt32(17),
                        champion8 = reader.GetInt32(18),
                        champion9 = reader.GetInt32(19),
                        champion10 = reader.GetInt32(20),
                        sideWin = reader.GetInt32(21),
                        ban1 = reader.GetInt32(22),
                        ban2 = reader.GetInt32(23),
                        ban3 = reader.GetInt32(24),
                        ban4 = reader.GetInt32(25),
                        ban5 = reader.GetInt32(26),
                        ban6 = reader.GetInt32(27),
                        ban7 = reader.GetInt32(28),
                        ban8 = reader.GetInt32(29),
                        ban9 = reader.GetInt32(30),
                        ban10 = reader.GetInt32(31),
                    });
                    
                } while (reader.Read());
            }
            catch (Exception e){ }
            _sqlConnection.Close();
            return matches;
        }

        public List<PlayerStats> ReadPlayerStats()
        {
            List<PlayerStats> players = new List<PlayerStats>();
            _sqlConnection.Open();
            string selectPlayerStatsSQL = "SELECT * FROM playerStats";
            try
            {
                SQLiteDataReader reader = new SQLiteCommand(selectPlayerStatsSQL, _sqlConnection).ExecuteReader();
                reader.Read();
                do
                {
                    players.Add(new PlayerStats
                    {
                        summonerId = reader.GetInt64(0),
                        playerName = reader.GetString(1),
                        leaguePoints = reader.GetInt32(2),
                        wins = reader.GetInt32(3),
                        losses = reader.GetInt32(4)
                    });
                } while (reader.Read());
            }
            catch (Exception e){ }
            _sqlConnection.Close();
            return players;
        }

        public PlayerChampionStats ReadPlayerChampionStats(long summonerId, int championId)
        {
            return new PlayerChampionStats();
        }
    }
}
