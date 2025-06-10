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
                var team = new EQTeamModel
                {
                    Player1Id = players[group[0] - 1].Id,
                    Player2Id = players[group[1] - 1].Id,
                    Player3Id = players[group[2] - 1].Id,
                    Player4Id = players[group[3] - 1].Id
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

        // Reorder rows handler (in-memory, not persisted in DB by default)
        public async Task<IActionResult> OnPostReorderAsync(int index, string direction)
        {
            Teams = await _context.EQTeams
                .Include(t => t.Player1)
                .Include(t => t.Player2)
                .Include(t => t.Player3)
                .Include(t => t.Player4)
                .OrderBy(t => t.Id)
                .ToListAsync();

            if ((direction == "up" && index > 0) || (direction == "down" && index < Teams.Count - 1))
            {
                int newIndex = direction == "up" ? index - 1 : index + 1;
                var temp = Teams[index];
                Teams[index] = Teams[newIndex];
                Teams[newIndex] = temp;
                // To persist: add a SortOrder property to EQTeamModel and update it here
            }

            return Page();
        }

        // Swap players within a team handler
        public async Task<IActionResult> OnPostSwapAsync(int teamIdx, int leftIdx, int rightIdx)
        {
            Teams = await _context.EQTeams
                .Include(t => t.Player1)
                .Include(t => t.Player2)
                .Include(t => t.Player3)
                .Include(t => t.Player4)
                .OrderBy(t => t.Id)
                .ToListAsync();

            if (teamIdx >= 0 && teamIdx < Teams.Count && leftIdx >= 0 && rightIdx <= 3 && leftIdx < rightIdx)
            {
                var team = Teams[teamIdx];
                var ids = new int[] {
                    team.Player1Id, team.Player2Id, team.Player3Id, team.Player4Id
                };
                // Swap the two
                var tmp = ids[leftIdx];
                ids[leftIdx] = ids[rightIdx];
                ids[rightIdx] = tmp;
                team.Player1Id = ids[0];
                team.Player2Id = ids[1];
                team.Player3Id = ids[2];
                team.Player4Id = ids[3];
                _context.EQTeams.Update(team);
                await _context.SaveChangesAsync();
            }

            return Page();
        }
    }
}
