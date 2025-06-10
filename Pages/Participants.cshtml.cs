using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tournoi.DB;

namespace Tournoi.Pages
{
    public class PlayerDisplay
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string City { get; set; }
        public Sex Sex { get; set; }
        public double Rank { get; set; }
        public bool SI { get; set; }
        public bool EQ { get; set; }
        public bool ASI { get; set; }
        public bool PSI { get; set; }
    }

    public class ParticipantsModel : PageModel
    {
        private readonly AppDBContext _context;

        public ParticipantsModel(AppDBContext context)
        {
            _context = context;
        }

        public List<PlayerDisplay> Participants { get; set; }

        public void OnGet()
        {
            Participants = _context.Players
                .Where(p => p.SI || p.EQ || p.ASI || p.PSI)
                .Include(p => p.Person)
                .Select(p => new PlayerDisplay
                {
                    Id = p.PersonId,
                    FullName = p.Person.FullName,
                    City = p.Person.City,
                    Sex = p.Person.Sex,
                    Rank = p.Person.Rank,
                    SI = p.SI,
                    ASI = p.ASI,
                    EQ = p.EQ,
                    PSI = p.PSI,
                }).ToList();
        }
    }
}
