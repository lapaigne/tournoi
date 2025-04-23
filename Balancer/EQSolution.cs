namespace TournoiServer.Balancer
{
    public struct EQSolution
    {
        public EQTeam[] Teams { get; set; }
        public readonly int Count => Teams.Length;
        public bool Complete { get; private set; }
    }
}
