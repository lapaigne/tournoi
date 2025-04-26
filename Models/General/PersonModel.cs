using System.ComponentModel.DataAnnotations.Schema;

namespace TournoiServer.Models
{
    [Table("People")]
    public class PersonModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";
        public double Rank { get; set; }
        public string City { get; set; } = "";
        public Sex Sex { get; set; }
    }
}
