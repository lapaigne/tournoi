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
                var name = Request.Form["name"];
                return HandleEditDisplayPlayer(name);
            }
            else if (action == "delete")
            {
                var name = Request.Form["name"];
                return HandleDeleteDisplayPlayer(name);
            }

            return BadRequest("Invalid action.");
        }

        private void LoadPlayerDisplays()
        {
            PlayerDisplays = _context.Players
                .Include(p => p.Person)
                .Select(p => new PlayerDisplay
                {
                    FullName = p.Person.FullName,
                    City = p.Person.City,
                    Sex = p.Person.Sex,
                    SI = p.SI,
                    EQ = p.EQ,
                    ASI = p.ASI,
                    PSI = p.PSI
                }).ToList();
        }

        private IActionResult HandleAddDisplayPlayer()
        {
            var person = new PersonModel
            {
                FullName = Request.Form["FullName"],
                City = Request.Form["City"],
                Sex = (Sex)int.Parse(Request.Form["Sex"])
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

        private IActionResult HandleEditDisplayPlayer(string name)
        {
            var person = _context.People.FirstOrDefault(p => p.FullName == name);
            if (person == null) return NotFound();

            var player = _context.Players.FirstOrDefault(p => p.PersonId == person.Id);

            person.City = Request.Form["City"];
            person.Sex = (Sex)int.Parse(Request.Form["Sex"]);

            if (player != null)
            {
                player.SI = Request.Form["SI"] == "on";
                player.EQ = Request.Form["EQ"] == "on";
                player.ASI = Request.Form["ASI"] == "on";
                player.PSI = Request.Form["PSI"] == "on";
            }

            _context.SaveChanges();
            LoadPlayerDisplays();
            return Partial("_PeopleTable", PlayerDisplays);
        }

        private IActionResult HandleDeleteDisplayPlayer(string name)
        {
            var person = _context.People.FirstOrDefault(p => p.FullName == name);
            if (person != null)
            {
                var player = _context.Players.FirstOrDefault(p => p.PersonId == person.Id);

                if (player != null) _context.Players.Remove(player);
                _context.People.Remove(person);

                _context.SaveChanges();
            }

            LoadPlayerDisplays();
            return Partial("_PeopleTable", PlayerDisplays);
        }
    }
}