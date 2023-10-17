using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models.WildBearCoffee;
using WildBearAdventuresMVC.WildBear;

namespace WildBearAdventuresMVC.Controllers
{



    public class WildBearCoffeeController : Controller
    {
        private readonly WildBearApiClient _wildBearApiClient;


        public WildBearCoffeeController(WildBearApiClient wildBearApiClient)
        {
            _wildBearApiClient = wildBearApiClient;
        }


        public IActionResult Index()
        {

            //TODO: replace dummy token
            var token = new CancellationToken();


            var productDto = _wildBearApiClient.GetRandomCoffeeProduct(token);

            var coffeeViewModel = new CoffeeViewModel()
            {
                Name = productDto.Name,

            };




            //TODO: Create View

            return View();
        }


        //Make it work first, then make it pretty, Then make it dynamic

    }
}
