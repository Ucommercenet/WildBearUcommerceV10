using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models;
using WildBearAdventuresMVC.WildBear.Interfaces;

namespace WildBearAdventuresMVC.Controllers
{

    public class ProductController : Controller
    {
        private readonly IWildBearApiClient _wildBearApiClient;
        private readonly IContextHelper _contextHelper;

        public ProductController(IWildBearApiClient wildBearApiClient, IContextHelper contextHelper)
        {
            _wildBearApiClient = wildBearApiClient;
            _contextHelper = contextHelper;
        }

        public IActionResult Index(CancellationToken token)
        {

            var ableToGetRoute = HttpContext.Request.RouteValues.TryGetValue("id", out var name);
            if (ableToGetRoute)
            {
                _contextHelper.SetCurrentProductByName(name.ToString());
            }

            var currentproductGuid = _contextHelper.GetCurrentProductGuid();

            if (currentproductGuid is null)
            { return View(); }

            var currentprodcutDto = _wildBearApiClient.GetSingleProductByGuid((Guid)currentproductGuid, token);



            var productViewModel = new ProductViewModel()
            {
                Name = currentprodcutDto.Name,
                ShortDescription = currentprodcutDto?.ShortDescription,
                Price = currentprodcutDto.UnitPrices.FirstOrDefault().Value,
            };



            return View(productViewModel);
        }
    }
}
