using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace BankApp
{
    public static class Database
    {
        private static string dbPath =
            Path.Combine(Application.StartupPath, "bank.db");

        private static string connectionString =
            $"Data Source={dbPath};Version=3;";

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        public static void Initialize()
        {
            if (!File.Exists(dbPath))
                SQLiteConnection.CreateFile(dbPath);

            using (var conn = GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();

                cmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS Users(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE,
                    Password TEXT,
                    Role TEXT,
                    Balance REAL
                );";
                cmd.ExecuteNonQuery();

                cmd.CommandText =
                @"INSERT OR IGNORE INTO Users
                  (Id,Username,Password,Role,Balance)
                  VALUES(1,'admin','1234','Admin',0);";
                cmd.ExecuteNonQuery();
            }
        }
    }
}