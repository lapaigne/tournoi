namespace TournoiServer.Models.Team
{
    public struct PSITeam
    {
        [DBPrimaryKey]
        public int Id { get; set; }
        public int Player1 { get; set; }
        public int Player2 { get; set; }
    }
}
