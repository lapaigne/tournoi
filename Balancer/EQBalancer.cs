using TournoiServer.Models;

namespace TournoiServer.Balancer
{
    public class EQBalancer
    {
        public readonly EQSolution solution;

        private EQPlayer[] _players;
        private int _shortCount;
        private bool _found;
        private ulong _forcedCities;
        private ulong[] _solution;
        private Dictionary<string, int> _cities = new Dictionary<string, int>();

        public EQBalancer()
        {
            throw new NotImplementedException();
        }

        public void PrepareData(PlayerModel[] players)
        {
            _shortCount = players.Length / 4;
            _solution = new ulong[_shortCount];

            foreach (PlayerModel player in players)
            {
                if (!_cities.TryAdd(player.City, 1))
                {
                    _cities[player.City]++;
                }
            }

            _players = players.Select(p =>
            {
                var player = new EQPlayer { };
                return player;
            }).ToArray();

            throw new NotImplementedException();
        }

        // search without any forced cities (cheaper)
        public void RegularSearch()
        {
            throw new NotImplementedException();
        }

        // search with forced cities (requires more checks)
        public void ForcedSearch()
        {
            throw new NotImplementedException();
        }
    }
}
