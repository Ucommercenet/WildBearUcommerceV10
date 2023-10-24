using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models;
using WildBearAdventuresMVC.WildBear;

namespace WildBearAdventuresMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IWildBearApiClient _wildBearApiClient;
        private readonly IContextHelper _contextHelper;


        public CategoryController(IWildBearApiClient wildBearApiClient, IContextHelper contextHelper)
        {
            _wildBearApiClient = wildBearApiClient;
            _contextHelper = contextHelper;
        }

        public IActionResult Index(CancellationToken token)
        {


            //TODO: Show sub-Categories

            //Test 0
            HttpContext.Session.SetString("TestKey", "42");
            var testValue = HttpContext.Session.GetString("TestKey");


            //Figure out currentCategory based on route values aka. how did we get here.
            //TODO: include the get Route in SetCurrentCategory()
            var ableToGetRoute = HttpContext.Request.RouteValues.TryGetValue("id", out var value);
            if (ableToGetRoute) { _contextHelper.SetCurrentCategory(value?.ToString()); }

            var currentCategoryGuid = _contextHelper.GetCurrentCategory();


            if (currentCategoryGuid is null)
            { return View(); }

            var productDtos = _wildBearApiClient.GetAllProductsFromCategoryGuid((Guid)currentCategoryGuid, token);

            var CategoryViewModel = new CategoryViewModel
            {
                ProductDtos = productDtos,
            };

            return View(CategoryViewModel);
        }
    }
}
