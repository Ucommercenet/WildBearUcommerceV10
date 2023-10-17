using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WildBearAdventuresMVC.Models;
using WildBearAdventuresMVC.WildBear;

namespace WildBearAdventuresMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WildBearApiClient _wildBearApiClient;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _wildBearApiClient = new WildBearApiClient();
        }
        public IActionResult Index()
        {
            var productDto = _wildBearApiClient.GetRandomCoffeeProduct(new CancellationToken());
            ViewBag.CoffeeName = productDto.Name;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
