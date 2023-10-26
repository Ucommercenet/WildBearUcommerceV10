using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models.WildBearCoffee;
using WildBearAdventuresMVC.WildBear.Interfaces;

namespace WildBearAdventuresMVC.Controllers
{
    public class CategoryNavigationController : Controller
    {
        private readonly IWildBearApiClient _wildBearApiClient;

        public CategoryNavigationController(IWildBearApiClient wildBearApiClient)
        {
            _wildBearApiClient = wildBearApiClient;
        }

        public IActionResult Index()
        {


            var categoryDtoCollection = _wildBearApiClient.GetAllCategoriesFromCatalog("MainProductCatalog", new CancellationToken());
            var categoryNavigationViewModel = new CategoryNavigationViewModel
            {
                categoryDtos = categoryDtoCollection
            };


            return View(categoryNavigationViewModel);
        }



    }
}
