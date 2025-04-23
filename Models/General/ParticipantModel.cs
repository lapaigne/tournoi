namespace TournoiServer.Models.General
{
    public class ParticipantModel
    {
        [DBForeignKey(TargerTableName = "players", TargetPropertyName = "Id")]
        public int Id { get; set; }
        public bool EQ { get; set; }
        public bool ASI { get; set; }
        public bool SI { get; set; }
        public bool PSI { get; set; }
    }
}
