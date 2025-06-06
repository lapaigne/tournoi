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
            // Remove existing teams (optional: only if you want to reset each time)
            var oldTeams = _context.EQTeams.ToList();
            if (oldTeams.Any())
            {
                _context.EQTeams.RemoveRange(oldTeams);
                await _context.SaveChangesAsync();
            }

            // Load players who can be balanced
            var players = _context.People
                .OrderByDescending(p => p.Rank)
                .ToArray();

            // Early out: only balance if divisible by 4
            if (players.Length < 4 || players.Length % 4 != 0)
            {
                ModelState.AddModelError(string.Empty, "Number of players must be divisible by 4.");
                Teams = new List<EQTeamModel>();
                return Page();
            }

            // Prepare and run balancer
            var balancer = new EQBalancer();
            balancer.PrepareData(players);
            balancer.StartSearch();

            // Collect solution
            var solution = balancer.GetSolution();
            var teamsToAdd = new List<EQTeamModel>();
            foreach (var group in solution)
            {
                // The group is a List<int> of player indices
                if (group.Count != 4) continue;
                var team = new EQTeamModel
                {
                    Player1Id = players[group[0]].Id,
                    Player2Id = players[group[1]].Id,
                    Player3Id = players[group[2]].Id,
                    Player4Id = players[group[3]].Id
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