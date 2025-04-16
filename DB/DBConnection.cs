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

            CreateTable<Player>("players");

            CreateTable<SIPlayer>("si_players");
            CreateTable<AntiSIPlayer>("anti_si_players");
            CreateTable<PairSIPlayer>("pair_si_players");
            CreateTable<EQPlayer>("eq_players");

            // create tables for result

            // create tables for play scheme

            // make storagee for intermediate data

            // populate data???
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
