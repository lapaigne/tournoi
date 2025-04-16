namespace TournoiServer.Models
{
    public struct EQTeams
    {
        [DBPrimaryKey]
        public int Id { get; set; }
        public int Player1 { get; set; }
        public int Player2 { get; set; }
        public int Player3 { get; set; }
        public int Player4 { get; set; }
    }
}
