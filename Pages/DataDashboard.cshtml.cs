using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tournoi.Pages
{
    [Authorize]
    public class DataDashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
