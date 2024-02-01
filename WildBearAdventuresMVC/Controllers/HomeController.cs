using Microsoft.AspNetCore.Mvc;
using WildBearAdventures.MVC.WildBear.TransactionApi;
using WildBearAdventures.MVC.ViewModels;

namespace WildBearAdventures.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStoreApiClient _wildBearApiClient;

        public HomeController(IStoreApiClient wildBearApiClient)
        {
            _wildBearApiClient = wildBearApiClient;
        }

        public IActionResult Index()
        {
            var productDto = _wildBearApiClient.GetRandomProductFromCategory(new Guid("7040940e-eab1-4a72-85b5-867905b7d94a"), new CancellationToken());
            productDto.UnitPrices.TryGetValue("EUR 15 pct", out var price);

            var coffeeViewModel = new CoffeeViewModel()
            {
                Name = productDto.Name,
                Price = price,
                Description = productDto.ShortDescription

            };

            return View(coffeeViewModel);
        }


    }
}
