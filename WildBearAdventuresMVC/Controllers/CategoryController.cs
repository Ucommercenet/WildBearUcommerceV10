using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models;
using WildBearAdventuresMVC.WildBear;

namespace WildBearAdventuresMVC.Controllers
{
    public class CategoryController : Controller
    {
        //TODO: Update to DI insted for new class
        private readonly WildBearApiClient _wildBearApiClient;

        public CategoryController()
        {
            _wildBearApiClient = new WildBearApiClient();
        }

        public IActionResult Index(CancellationToken token)
        {

            //TODO: Show sub-Categories


            //Figure out currentCategory based on route values            
            var ableToGetCurrent = HttpContext.Request.RouteValues.TryGetValue("id", out var value);
            var currentCategory = value?.ToString();








            var WildCoffeeCategory = new Guid("7040940e-eab1-4a72-85b5-867905b7d94a");

            //TODO: Do not  just show WildCoffee Category -- Make this dynamic
            //currentCategory = WildCoffeeCategory;

            var productDtos = _wildBearApiClient.GetAllProductsFromCategoryByGuid(WildCoffeeCategory, token);

            var CategoryViewModel = new CategoryViewModel
            {
                ProductDtos = productDtos,
            };



            //TODO: save currenctProduct in cookie
            HttpContext.Response.Cookies.Append("ProductId", "42");





            return View(CategoryViewModel);
        }
    }
}
