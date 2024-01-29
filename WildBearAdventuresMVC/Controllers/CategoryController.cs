using Microsoft.AspNetCore.Mvc;
using WildBearAdventures.MVC.WildBear.Context;
using WildBearAdventures.MVC.WildBear.TransactionApi;
using WildBearAdventures.MVC.ViewModel;


namespace WildBearAdventures.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IStoreApiClient _wildBearApiClient;
        private readonly IContextHelper _contextHelper;


        public CategoryController(IStoreApiClient wildBearApiClient, IContextHelper contextHelper)
        {
            _wildBearApiClient = wildBearApiClient;
            _contextHelper = contextHelper;
        }

        public IActionResult Index(string id, CancellationToken token)
        {
            //TODO Improvement: Show sub-Categories                        

            //Figure out currentCategory based on route values aka. how did we get here.
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
                CurrentCategoryName = currentCategoryDto is not null ? currentCategoryDto?.Name : "No currentCategory",
            };

            return View(CategoryViewModel);
        }
    }
}
