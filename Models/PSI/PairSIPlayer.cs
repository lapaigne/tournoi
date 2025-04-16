namespace TournoiServer.Models
{
    public struct PairSIPlayer
    {
        [DBPrimaryKey]
        public int Id { get; set; }
        public string City { get; set; }
        public Sex Sex { get; set; }
    }
}
