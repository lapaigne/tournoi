namespace Tournoi.Models
{
    public class PairModel
    {
        public int Id { get; set; }

        public int Player1Id { get; set; }
        public int Player2Id { get; set; }

        public PersonModel Player1 { get; set; }
        public PersonModel Player2 { get; set; }
    }
}
