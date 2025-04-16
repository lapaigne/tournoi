namespace TournoiServer.Models
{
    public struct Player
    {
        public Player() { }

        [DBPrimaryKey]
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public double Rank { get; set; }
        public string City { get; set; } = string.Empty;
        public Sex Sex { get; set; }
    }
}
