using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models;
using WildBearAdventuresMVC.WildBear.Interfaces;

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

        public IActionResult Index(string id, CancellationToken token)
        {
            //TODO: Show sub-Categories                        

            //Figure out currentCategory based on route values aka. how did we get here.
            //TODO: include the get Route in SetCurrentCategory()
            var ableToGetRoute = HttpContext.Request.RouteValues.TryGetValue("id", out var routeName);
            if (ableToGetRoute)
            {
                _contextHelper.SetCurrentCategoryByName(routeName?.ToString());
            }


            var currentCategoryGuid = _contextHelper.GetCurrentCategoryGuid();


            if (currentCategoryGuid is null)
            { return View(); }


            var currentCategoryDto = _wildBearApiClient.GetSingleCategoryByGuid((Guid)currentCategoryGuid, token);

            var productDtos = _wildBearApiClient.GetAllProductsFromCategoryGuid((Guid)currentCategoryGuid, token);

            var CategoryViewModel = new CategoryViewModel
            {
                ProductDtos = productDtos,
                CurrentCategoryName = (currentCategoryDto is not null) ? currentCategoryDto?.Name : "No currentCategory",
            };

            return View(CategoryViewModel);
        }
    }
}
