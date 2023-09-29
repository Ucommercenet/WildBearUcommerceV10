using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models.WildBearCoffee;

namespace WildBearAdventuresMVC.Controllers
{
    public class WildBearCoffeeController : Controller
    {

        public IActionResult Index()
        {
            var model = new CoffeeViewModel();


            var ucommerceResponse = CallUcommerceApi();




            //TODO: Create View

            return View();
        }


        //Make it work first, then make it pretty, Then make it dynamic
        private HttpResponseMessage CallUcommerceApi()
        {
            var host = HttpContext.Request.Host.Host;
            var port = HttpContext.Request.Host.Port;

            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod(
                                method: "GET"),
                            requestUri: @$"https://localhost:44381/api/CoffeeScenarios/GetAllProductsFromCategoryWildCoffee");


            var response = client.Send(request);

            return response;




        }
    }
}
