namespace TournoiServer.Models
{
    public struct AntiSIPlayerModel
    {
        [DBPrimaryKey]
        public int Id { get; set; }
        public double Rank { get; set; }
        public string City { get; set; }
        public Sex Sex { get; set; }
    }
}
