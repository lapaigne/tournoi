namespace TournoiServer.Models
{
    public struct SIPlayer
    {
        [DBPrimaryKey]
        public int Id { get; set; }
        public Rank Rank { get; set; }
        public string City { get; set; }
        public Sex Sex { get; set; }
    }
}
