namespace TournoiServer.Models
{
    public struct PairSIPlayerModel
    {
        [DBPrimaryKey]
        public int Id { get; set; }
        public string City { get; set; }
        public Sex Sex { get; set; }
    }
}
