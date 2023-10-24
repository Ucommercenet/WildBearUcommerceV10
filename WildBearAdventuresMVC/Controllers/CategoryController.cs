using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models;
using WildBearAdventuresMVC.WildBear;

namespace WildBearAdventuresMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IWildBearApiClient _wildBearApiClient;

        public CategoryController(IWildBearApiClient wildBearApiClient)
        {
            _wildBearApiClient = wildBearApiClient;
        }

        public IActionResult Index(CancellationToken token)
        {

            //TODO: Show sub-Categories


            //Figure out currentCategory based on route values            
            var ableToGetCurrent = HttpContext.Request.RouteValues.TryGetValue("id", out var value);
            var currentCategory = value?.ToString();



            var currentCategoryGuid = _wildBearApiClient.GetOnlyGuidByName(currentCategory, token);




            //var WildCoffeeCategory = new Guid("7040940e-eab1-4a72-85b5-867905b7d94a");

            //TODO: Do not  just show WildCoffee Category -- Make this dynamic
            //currentCategory = WildCoffeeCategory;

            var productDtos = _wildBearApiClient.GetAllProductsFromCategoryGuid(currentCategoryGuid, token);

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
