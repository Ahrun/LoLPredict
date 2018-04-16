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

        public MatchModel ReadMatch(long matchId)
        {
            return new MatchModel();
        }

        public PlayerStats ReadPlayerStats(long summonerId)
        {
            return new PlayerStats();
        }

        public PlayerChampionStats ReadPlayerChampionStats(long summonerId, int championId)
        {
            return new PlayerChampionStats();
        }
    }
}
