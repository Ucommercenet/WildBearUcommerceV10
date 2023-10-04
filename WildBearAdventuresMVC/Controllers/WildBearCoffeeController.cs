using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WildBearAdventuresMVC.Models.WildBearCoffee;
using WildBearAdventuresMVC.WildBearRelated.DTO;

namespace WildBearAdventuresMVC.Controllers
{
    public class WildBearCoffeeController : Controller
    {

        public IActionResult Index()
        {
            var coffeeViewModel = new CoffeeViewModel();


            var ucommerceResponse = CallApiForProducts();


            //Do Deserialize ucommerceResponse into model

            var deserializedObject = JsonConvert.DeserializeObject<SimpleUcommerceProductDTO>(ucommerceResponse);

            coffeeViewModel.Name = deserializedObject?.name;


            //TODO: Create View

            return View();
        }


        //Make it work first, then make it pretty, Then make it dynamic
        private string CallApiForProducts()
        {
            var token = new CancellationToken();
            var host = HttpContext.Request.Host.Host;
            var port = HttpContext.Request.Host.Port;

            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod(
                                method: "GET"),
                            requestUri: @$"https://localhost:7255/api/Product/WildCoffee");


            var response = client.GetAsync("https://localhost:7255/api/Product/WildCoffee").Result;




            return response.Content.ReadAsStringAsync().Result;







        }
    }
}
