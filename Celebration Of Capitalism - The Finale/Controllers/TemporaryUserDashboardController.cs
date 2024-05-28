using Microsoft.AspNetCore.Mvc;

namespace Celebration_Of_Capitalism___The_Finale.Controllers
{
    public class TemporaryUserDashboardController : Controller
    {
		public IActionResult Index()
        {
            return View();
        }
    }
}
