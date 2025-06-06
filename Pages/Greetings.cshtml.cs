using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tournoi.DB;
using Tournoi.Models;

namespace Tournoi.Pages
{
    public class GreetingsModel : PageModel
    {
        private readonly AppDBContext _context;

        public GreetingsModel(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (Request.IsHtmx())
            {
                string testString = "THIS IS A TEST";
                PersonModel[] people =
                [
                    new PersonModel { FullName = "ABC", City = "MSC", Rank = 5325.33, Sex = Sex.Male },
                ];

                _context.People.AddRange(people);
                _context.SaveChanges();

                return Content($"<span>{testString}</span>", "text/html");
            }

            return Page();
        }
    }
}