using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models;
using WildBearAdventuresMVC.WildBear;

namespace WildBearAdventuresMVC.Controllers
{

    public class ProductController : Controller
    {
        private readonly IWildBearApiClient _wildBearApiClient;

        public ProductController(IWildBearApiClient wildBearApiClient)
        {
            _wildBearApiClient = wildBearApiClient;
        }

        public IActionResult Index()
        {

            var Request = HttpContext.Request;


            //Byte-Size Espresso -- "2d18ae3e-aa99-4246-8cec-9b027b7c1f13"

            var currenctProduct = "";

            var productViewModel = new ProductViewModel() { };



            return View();
        }
    }
}
