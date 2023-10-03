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


            //Do Deserialize ucommerceResponse into model





            //TODO: Create View

            return View();
        }


        //Make it work first, then make it pretty, Then make it dynamic
        private async string CallUcommerceApi()
        {
            var token = new CancellationToken();
            var host = HttpContext.Request.Host.Host;
            var port = HttpContext.Request.Host.Port;

            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod(
                                method: "GET"),
                            requestUri: @$"https://localhost:7255/api/Product/WildCoffee");


            var response = client.GetAsync("https://localhost:7255/api/Product/WildCoffee");



            var content = await response.Result.Content.ReadAsStringAsync();






        }
    }
}
