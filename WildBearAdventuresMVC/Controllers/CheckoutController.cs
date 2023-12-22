using Microsoft.AspNetCore.Mvc;

namespace WildBearAdventuresMVC.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
