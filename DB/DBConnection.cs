using Microsoft.Data.Sqlite;
using TournoiServer.Models;

namespace TournoiServer.DB
{
    public partial class DBConnection
    {
        private static DBConnection _instance;
        private SqliteConnection _connection;

        private DBConnection()
        {
            _connection = new SqliteConnection("Data Source=database.db");
            _connection.Open();

            CreateTable<PlayerModel>("players");

            // create tables for result

            // create tables for play scheme

            // make storagee for intermediate data

            // populate data???
            PlayerModel[] players = [new PlayerModel { City = "MSC", FullName = "first" }, new PlayerModel { City = "SPB", FullName = "second" }];
            AddRows("players", players);

            GetTable<PlayerModel>("players");
            GetTable("players", "FullName", "City");
        }

        public static DBConnection Instance
        {
            get { return _instance ??= new DBConnection(); }
        }

        public SqliteConnection Connection
        {
            get { return _connection; }
        }
    }
}
