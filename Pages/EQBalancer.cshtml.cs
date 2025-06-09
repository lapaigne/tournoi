using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tournoi.Balancer;
using Tournoi.DB;
using Tournoi.Models;

namespace Tournoi.Pages
{
    public class EQBalancerModel : PageModel
    {
        private readonly AppDBContext _context;

        public EQBalancerModel(AppDBContext context)
        {
            _context = context;
        }

        public List<EQTeamModel> Teams { get; set; } = new();

        public async Task OnGetAsync()
        {
            Teams = await _context.EQTeams
                .Include(t => t.Player1)
                .Include(t => t.Player2)
                .Include(t => t.Player3)
                .Include(t => t.Player4)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostGenerateAsync()
        {
            var oldTeams = _context.EQTeams.ToList();
            if (oldTeams.Any())
            {
                _context.EQTeams.RemoveRange(oldTeams);
                await _context.SaveChangesAsync();
            }

            var players = _context.People.ToArray();

            if (players.Length < 4 || players.Length % 4 != 0)
            {
                ModelState.AddModelError(string.Empty, "Number of players must be divisible by 4.");
                Teams = new List<EQTeamModel>();
                return Page();
            }

            var balancer = new EQBalancer();
            balancer.PrepareData(players);
            balancer.StartSearch();

            var solution = balancer.GetSolution();
            var teamsToAdd = new List<EQTeamModel>();

            foreach (var group in solution)
            {
                var hash = group.GetHashCode();
                var team = new EQTeamModel
                {
                    Player1Id = players[group[hash % 4] - 1].Id,
                    Player2Id = players[group[(1 + hash) % 4] - 1].Id,
                    Player3Id = players[group[(2 + hash) % 4] - 1].Id,
                    Player4Id = players[group[(3 + hash) % 4] - 1].Id
                };
                teamsToAdd.Add(team);
            }

            if (teamsToAdd.Any())
            {
                _context.EQTeams.AddRange(teamsToAdd);
                await _context.SaveChangesAsync();
            }

            Teams = await _context.EQTeams
                .Include(t => t.Player1)
                .Include(t => t.Player2)
                .Include(t => t.Player3)
                .Include(t => t.Player4)
                .ToListAsync();

            return Page();
        }
    }
}