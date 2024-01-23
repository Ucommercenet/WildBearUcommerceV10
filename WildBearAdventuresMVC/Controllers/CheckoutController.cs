using Microsoft.AspNetCore.Mvc;

namespace WildBearAdventures.MVC.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
