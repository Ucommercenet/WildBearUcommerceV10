using Microsoft.AspNetCore.Mvc;
using WildBearAdventures.MVC.WildBear.Context;
using WildBearAdventures.MVC.WildBear.TransactionApi;
using WildBearAdventures.MVC.ViewModels;


namespace WildBearAdventures.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IStoreApiClient _wildBearClient;
        private readonly IContextHelper _contextHelper;


        public CategoryController(IStoreApiClient wildBearClient, IContextHelper contextHelper)
        {
            _wildBearClient = wildBearClient;
            _contextHelper = contextHelper;
        }

        public IActionResult Index(string id, CancellationToken token)
        {
            //Handout Part 2.2: Category details
            #region MyRegion
            //TODO Improvement: Handle sub-Categories too                     

            _contextHelper.SetCurrentCategoryByName(id);
            var currentCategoryGuid = _contextHelper.GetCurrentCategoryGuid();


            if (currentCategoryGuid is null)
            { return View(); }
            
            var currentCategoryDto = _wildBearClient.GetSingleCategoryByGuid((Guid)currentCategoryGuid, token);
            
            var productDtos = _wildBearClient.GetAllProductsFromCategoryGuid((Guid)currentCategoryGuid, token);

            var CategoryViewModel = new CategoryViewModel
            {
                ProductDtos = productDtos,
                CurrentCategoryName = currentCategoryDto is not null ? currentCategoryDto?.Name : "No currentCategory",
            };

            return View(CategoryViewModel); 
            #endregion
            
        }
    }
}
