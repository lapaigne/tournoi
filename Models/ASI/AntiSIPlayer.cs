namespace TournoiServer.Models
{
    public struct AntiSIPlayer
    {
        [DBPrimaryKey]
        public int Id { get; set; }
        public Rank Rank { get; set; }
        public string City { get; set; }
        public Sex Gender { get; set; }
    }
}
