using Microsoft.AspNetCore.Mvc;
using WildBear.Shop.Coffee.WildBear.TransactionApi;
using WildBearAdventures.MVC.ViewModels;

namespace WildBear.Shop.Coffee.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStoreApiClient _wildBearClient;

        public HomeController(IStoreApiClient wildBearClient)
        {
            _wildBearClient = wildBearClient;
        }

        public IActionResult Index()
        {
            //Handout Part 1: (Extra) Write an Ucommerce endpoint

            var productDto = _wildBearClient.GetRandomProductFromCategory(categoryName: "Drinks", new CancellationToken());
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
