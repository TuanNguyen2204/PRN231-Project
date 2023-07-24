using eClothesClient.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace eClothesClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RedirectUnauthenticated]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
