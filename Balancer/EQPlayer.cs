namespace TournoiServer.Balancer
{
    public struct EQPlayer
    {
        public ulong IndexMask { get; set; }
        public ulong CityMask { get; set; }
        public double Rank { get; set; }
        public Sex Sex { get; set; }
    }
}
