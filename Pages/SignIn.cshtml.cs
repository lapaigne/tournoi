using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Tournoi.Pages
{
    public class SignInModel : PageModel
    {
        [BindProperty]
        public string Token { get; set; }
        private string _token = "234";

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Token != _token)
            {
                ModelState.AddModelError("", "Неправильный токен");
                return Partial("SignIn", this);
            }

            List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Role, "j-_-") };
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(3),
            });

            Response.Headers["HX-Redirect"] = Url.Page("/Index");
            return new EmptyResult();
        }
    }
}
