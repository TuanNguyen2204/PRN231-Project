using Microsoft.AspNetCore.Mvc;

namespace eClothesClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
