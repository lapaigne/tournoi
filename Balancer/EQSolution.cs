namespace TournoiServer.Balancer
{
    public class EQSolution
    {
        public required ulong[] Teams { get; set; }
        public bool Complete { get; set; }
        public int Count => Teams.Length;
    }
}
