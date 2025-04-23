namespace TournoiServer.Models
{
    public struct PlayerModel
    {
        [DBPrimaryKey]
        public int Id { get; set; }
        public string FullName { get; set; }
        public double Rank { get; set; }
        public string City { get; set; }
        public Sex Sex { get; set; }
    }
}
