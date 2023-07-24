using Microsoft.AspNetCore.Mvc;

namespace eClothesClient.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
