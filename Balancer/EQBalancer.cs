using TournoiServer.Models;

namespace TournoiServer.Balancer
{
    public class EQBalancer
    {
        public EQSolution Solution { get; private set; }
        public EQPlayer[] Players { get; private set; }

        private int _shortCount;
        private bool _found;
        private ulong _forcedCities;
        private Dictionary<ulong, int> _cities = new Dictionary<ulong, int>();

        public EQBalancer(PlayerModel[] players)
        {
            throw new NotImplementedException();
        }

        public void PrepareData()
        {
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
