using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartsApp
{
    class DataBase
	{
		private static string databasePath = "stats.db";
		private static string connectionString = $"Data Source={databasePath};Version=3;";

		public static void Initialize()
		{
			if (!File.Exists(databasePath))
			{
				SQLiteConnection.CreateFile(databasePath);
				using (var connection = new SQLiteConnection(connectionString))
				{
					connection.Open();
					string createTableQuery = @"
                    CREATE TABLE Statistics (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        WindowsCount INTEGER NOT NULL,
                        MacCount INTEGER NOT NULL,
                        LinuxCount INTEGER NOT NULL,
                        Date TEXT NOT NULL
                    )";
					new SQLiteCommand(createTableQuery, connection).ExecuteNonQuery();
				}
			}
		}

		public static void AddEntry(int windows, int mac, int linux, DateTime date)
		{
			using (var connection = new SQLiteConnection(connectionString))
			{
				connection.Open();
				string insertQuery = @"
                INSERT INTO Statistics (WindowsCount, MacCount, LinuxCount, Date)
                VALUES (@win, @mac, @linux, @date)";
				var cmd = new SQLiteCommand(insertQuery, connection);
				cmd.Parameters.AddWithValue("@win", windows);
				cmd.Parameters.AddWithValue("@mac", mac);
				cmd.Parameters.AddWithValue("@linux", linux);
				cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
				cmd.ExecuteNonQuery();
			}
		}

		public static List<StatsEntry> GetAllEntries()
		{
			var entries = new List<StatsEntry>();
			using (var connection = new SQLiteConnection(connectionString))
			{
				connection.Open();
				string selectQuery = "SELECT * FROM Statistics ORDER BY Date DESC";
				using (var cmd = new SQLiteCommand(selectQuery, connection))
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						entries.Add(new StatsEntry
						{
							Windows = Convert.ToInt32(reader["WindowsCount"]),
							Mac = Convert.ToInt32(reader["MacCount"]),
							Linux = Convert.ToInt32(reader["LinuxCount"]),
							Date = DateTime.Parse(reader["Date"].ToString())
						});
					}
				}
			}
			return entries;
		}
		public static StatsEntry GetEntryByDate(DateTime date)
		{
			using (var connection = new SQLiteConnection(connectionString))
			{
				connection.Open();
				string query = "SELECT * FROM Statistics WHERE Date = @date LIMIT 1";
				var cmd = new SQLiteCommand(query, connection);
				cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						return new StatsEntry
						{
							Windows = Convert.ToInt32(reader["WindowsCount"]),
							Mac = Convert.ToInt32(reader["MacCount"]),
							Linux = Convert.ToInt32(reader["LinuxCount"]),
							Date = DateTime.Parse(reader["Date"].ToString())
						};
					}
				}
			}
			return null;
		}
	}

	public class StatsEntry
	{
		public int Windows { get; set; }
		public int Mac { get; set; }
		public int Linux { get; set; }
		public DateTime Date { get; set; }
	}

}
