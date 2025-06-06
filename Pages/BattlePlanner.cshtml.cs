using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tournoi.Battles;
using Tournoi.DB;
namespace Tournoi.Pages
{
    public class ViewSchemesModel : PageModel
    {
        private readonly AppDBContext _context;

        public List<List<string>> Battles { get; set; } = new();
        public List<string> SelectedPlayers { get; set; } = new();

        public ViewSchemesModel(AppDBContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var players = _context.Players
                .Include(p => p.Person)
                .Where(p => p.SI)
                .OrderBy(p => EF.Functions.Random())
                .Take(7)
                .Select(p => p.Person.FullName)
                .ToList();

            SelectedPlayers = players;

            if (players.Count == 7)
            {
                var planner = new BattlePlanner();
                var scheme = planner.ApplyScheme("seven", players.ToArray());

                Battles = scheme.Select(row => row.ToList()).ToList();
            }
        }
    }
}