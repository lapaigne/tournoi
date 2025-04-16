using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace tournoi_server.Pages
{
    public class Greetings : PageModel
    {
        public IActionResult OnGet()
        {
            return Request.IsHtmx() ? Content($"<span>Bruh</span>", "text/html") : Page();
        }
    }
}
