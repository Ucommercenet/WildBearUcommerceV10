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

            //TODO: Model must have all sup-catogire if any
            //TODO: Model must have all prodcuts -- ok?

            var WildCoffeeCategory = new Guid("7040940e-eab1-4a72-85b5-867905b7d94a");

            var productDtos = _wildBearApiClient.GetAllProductsFromCategoryByGuid(WildCoffeeCategory, token);

            var CategoryViewModel = new CategoryViewModel
            {
                ProductDtos = productDtos,
            };



            return View(CategoryViewModel);
        }
    }
}
