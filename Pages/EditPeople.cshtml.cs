using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tournoi.DB;
using Tournoi.Models;

namespace Tournoi.Pages
{
    public class EditPeopleModel : PageModel
    {
        private readonly AppDBContext _context;

        public EditPeopleModel(AppDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<PlayerDisplay> PlayerDisplays { get; set; }

        [BindProperty]
        public PlayerDisplay Player { get; set; }

        public IActionResult OnGet()
        {
            LoadPlayerDisplays();
            return Page();
        }

        public IActionResult OnPost()
        {
            var action = Request.Form["action"];
            if (action == "add")
            {
                return HandleAddDisplayPlayer();
            }
            else if (action == "edit")
            {
                return HandleEditDisplayPlayer();
            }
            else if (action == "delete")
            {
                return HandleDeleteDisplayPlayer();
            }

            return BadRequest("Invalid action.");
        }

        // Load players into the display model
        private void LoadPlayerDisplays()
        {
            PlayerDisplays = _context.Players
                .Include(p => p.Person)
                .Select(p => new PlayerDisplay
                {
                    Id = p.PersonId,
                    FullName = p.Person.FullName,
                    City = p.Person.City,
                    Rank = p.Person.Rank,
                    Sex = p.Person.Sex,
                    SI = p.SI,
                    EQ = p.EQ,
                    ASI = p.ASI,
                    PSI = p.PSI
                }).ToList();
        }

        // Add a new player
        private IActionResult HandleAddDisplayPlayer()
        {
            var person = new PersonModel
            {
                FullName = Request.Form["FullName"],
                City = Request.Form["City"],
                Sex = (Sex)int.Parse(Request.Form["Sex"]),
                Rank = int.Parse(Request.Form["Rank"]),
            };

            var player = new PlayerModel
            {
                SI = Request.Form["SI"] == "on",
                EQ = Request.Form["EQ"] == "on",
                ASI = Request.Form["ASI"] == "on",
                PSI = Request.Form["PSI"] == "on"
            };

            _context.People.Add(person);
            _context.SaveChanges();

            player.PersonId = person.Id;
            _context.Players.Add(player);
            _context.SaveChanges();

            LoadPlayerDisplays();
            return Partial("_PeopleTable", PlayerDisplays);
        }

        // Edit an existing player
        private IActionResult HandleEditDisplayPlayer()
        {
            int id = int.Parse(Request.Form["Id"]);
            var player = _context.Players.Include(p => p.Person).FirstOrDefault(p => p.PersonId == id);

            if (player == null) return NotFound();

            player.Person.FullName = Request.Form["FullName"];
            player.Person.City = Request.Form["City"];
            player.Person.Sex = (Sex)int.Parse(Request.Form["Sex"]);
            player.Person.Rank = int.Parse(Request.Form["Rank"]);
            player.SI = Request.Form["SI"] == "on";
            player.EQ = Request.Form["EQ"] == "on";
            player.ASI = Request.Form["ASI"] == "on";
            player.PSI = Request.Form["PSI"] == "on";

            _context.SaveChanges();
            LoadPlayerDisplays();
            return Partial("_PeopleTable", PlayerDisplays);
        }

        // Delete a player
        private IActionResult HandleDeleteDisplayPlayer()
        {
            int id = int.Parse(Request.Form["Id"]);
            var player = _context.Players.Include(p => p.Person).FirstOrDefault(p => p.PersonId == id);

            if (player != null)
            {
                _context.Players.Remove(player);
                _context.People.Remove(player.Person);
                _context.SaveChanges();
            }

            LoadPlayerDisplays();
            return Partial("_PeopleTable", PlayerDisplays);
        }

        // Fetch a player for editing
        public IActionResult OnPostGetPlayer([FromBody] int id)
        {
            var player = _context.Players.Include(p => p.Person).FirstOrDefault(p => p.PersonId == id);
            if (player == null) return NotFound();

            Player = new PlayerDisplay
            {
                Id = player.PersonId,
                FullName = player.Person.FullName,
                City = player.Person.City,
                Rank = player.Person.Rank,
                Sex = player.Person.Sex,
                SI = player.SI,
                EQ = player.EQ,
                ASI = player.ASI,
                PSI = player.PSI
            };

            return Partial("EditPlayerPanel", Player); // Replace "_PlayerForm" with the proper form partial view name
        }
    }
}