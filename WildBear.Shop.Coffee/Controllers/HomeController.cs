using Microsoft.AspNetCore.Mvc;
using WildBearAdventures.MVC.ViewModels;
using WildBearAdventures.MVC.WildBear.TransactionApi;

namespace WildBearAdventures.MVC.Controllers
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
