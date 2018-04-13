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

        }
    }
}
