namespace TournoiServer.Balancer
{
    public struct EQTeam
    {
        public ulong cities;
        public ulong available;
        public ulong members;
        public int index;
        public int count;
        public int third;
        // TODO: add field for sex for balancing

        public int[] GetPlayers()
        {
            throw new NotImplementedException();
        }

        public void AddPlayer(EQPlayer player)
        {
            throw new NotImplementedException();
        }

        public void RemovePlayer(EQPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}
