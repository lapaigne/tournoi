namespace Tournoi.Models
{
    public class EQTeamModel
    {
        public int Id { get; set; }

        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public int Player3Id { get; set; }
        public int Player4Id { get; set; }

        public PersonModel Player1 { get; set; }
        public PersonModel Player2 { get; set; }
        public PersonModel Player3 { get; set; }
        public PersonModel Player4 { get; set; }
    }
}
