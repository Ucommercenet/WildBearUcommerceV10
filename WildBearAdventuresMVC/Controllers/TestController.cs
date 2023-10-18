using Microsoft.AspNetCore.Mvc;

namespace WildBearAdventuresMVC.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {

            var test = "Hello";

            return View();
        }
    }
}
