using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace Bolnisa.Database
{
    public class DatabaseHelper
    {
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HospitalDB.db");
                    _connectionString = $"Data Source={dbPath}";
                }
                return _connectionString;
            }
        }

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(ConnectionString);
        }

        public static void ExecuteNonQuery(string query, Action<SqliteCommand> addParameters = null)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    addParameters?.Invoke(command);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static T ExecuteScalar<T>(string query, Action<SqliteCommand> addParameters = null)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    addParameters?.Invoke(command);
                    var result = command.ExecuteScalar();
                    return result == DBNull.Value ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                }
            }
        }
    }
}