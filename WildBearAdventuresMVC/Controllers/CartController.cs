using Microsoft.AspNetCore.Mvc;

namespace WildBearAdventuresMVC.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
