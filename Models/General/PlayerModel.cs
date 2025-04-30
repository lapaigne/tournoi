namespace Tournoi.Models
{
    public class PlayerModel
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public bool EQ { get; set; }
        public bool ASI { get; set; }
        public bool SI { get; set; }
        public bool PSI { get; set; }

        public PersonModel Person { get; set; }
    }
}
