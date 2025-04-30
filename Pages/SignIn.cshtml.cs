using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tournoi.Pages
{
    public class SignInModel : PageModel
    {
        [BindProperty]
        public string Password { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            Console.WriteLine(Password);
            return Content("<p>Woah, you pressed the button and it worked! So cool, isn't it?<p>", "text/html");
        }
    }
}
