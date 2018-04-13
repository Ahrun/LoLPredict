using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace DataScraper
{
    public class DatabaseAccess
    {
        SQLiteConnection _sqlConnection;

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
            string createMatchTableSQL = "CREATE TABLE match (matchId INTEGER PRIMARY KEY, summoner1 INTEGER, summoner2 INTEGER, summoner3 INTEGER, summoner 4 INTEGER, summoner 5 INTEGER, " +
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
                "sideWin"
        }

        public void WritePlayerStats(PlayerStats playerStats)
        {

        }

        public void WritePlayerChampionStats(PlayerChampionStats playerChampionStats)
        {

        }

        public MatchModel ReadMatch(long matchId)
        {

        }

        public PlayerStats ReadPlayerStats(long summonerId)
        {

        }

        public PlayerChampionStats ReadPlayerChampionStats(long summonerId, int championId)
        {
            
        }
    }
}
