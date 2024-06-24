using Microsoft.AspNetCore.Mvc;
using WildBear.Shop.Coffee.ViewModels;
using WildBear.Shop.Coffee.WildBear.TransactionApi;


namespace WildBear.Shop.Coffee.Controllers
{
    public class CategoryNavigationController : Controller
    {
        private readonly IStoreApiClient _wildBearClient;

        public CategoryNavigationController(IStoreApiClient wildBearClient)
        {
            _wildBearClient = wildBearClient;
        }

        public IActionResult Index()
        {
            //Handout Part 2.1: Category navigation
            #region Handout
            var categoryDtoCollection = _wildBearClient.GetAllCategoriesFromCatalog("uCommerce", new CancellationToken());

            var categoryNames = new List<string>();

            foreach (var categoryDto in categoryDtoCollection)
            {
                categoryNames.Add(categoryDto.Name);
            }

            var categoryNavigationViewModel = new CategoryNavigationViewModel
            {
                categoryNames = categoryNames
            };


            return View(categoryNavigationViewModel);
            #endregion


        }



    }
}
