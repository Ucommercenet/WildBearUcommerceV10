using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WildBearAdventuresMVC.Models;
using WildBearAdventuresMVC.Models.WildBearCoffee;
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



            var productDto = _wildBearApiClient.GetRandomProductFromCategory("WildCoffee", new CancellationToken());

            var coffeeViewModel = new CoffeeViewModel()
            {
                Name = productDto.Name,
                Price = productDto.PricesInclTax.FirstOrDefault().Value,
                Description = productDto.ShortDescription

            };



            return View(coffeeViewModel);
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
