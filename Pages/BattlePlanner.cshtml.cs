using Microsoft.AspNetCore.Mvc.RazorPages;
using Tournoi.Battles;

namespace Tournoi.Pages
{
    public class BattlePlanerModel : PageModel
    {
        private readonly BattlePlanner _planner;

        public BattlePlanerModel(BattlePlanner planner)
        {
            _planner = planner;
        }

        public void OnGet()
        {
            string[] names = ["À", "Á", "Â", "Ã", "Ä", "Å"];
            string[][] battles = _planner.ApplyScheme("six", names);
            foreach (string[] battle in battles)
            {
                Console.WriteLine(string.Join("\t", battle));
            }
        }
    }
}
