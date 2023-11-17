using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models;
using WildBearAdventuresMVC.WildBear;
using WildBearAdventuresMVC.WildBear.Interfaces;
using WildBearAdventuresMVC.WildBear.TransactionApi;

namespace WildBearAdventuresMVC.Controllers
{

    public class ProductController : Controller
    {
        private readonly IWildBearApiClient _wildBearApiClient;
        private readonly IContextHelper _contextHelper;
        private readonly ITransactionClient _transactionClient;

        public ProductController(IWildBearApiClient wildBearApiClient, IContextHelper contextHelper, ITransactionClient transactionClient)
        {
            _wildBearApiClient = wildBearApiClient;
            _contextHelper = contextHelper;
            _transactionClient = transactionClient;
        }

        [HttpGet]
        public IActionResult Index(CancellationToken ct)
        {

            var ableToGetRoute = HttpContext.Request.RouteValues.TryGetValue("id", out var name);
            if (ableToGetRoute)
            { _contextHelper.SetCurrentProductByName(name.ToString()); }

            var productViewModel = CreateProductViewModel(ct);


            return View(productViewModel);
        }

        [HttpPost]
        public RedirectToActionResult AddToCart(CancellationToken ct)
        {

            var currency = "DKK";
            var cultureCode = "en-DK";

            var currentproduct = _contextHelper.GetCurrentProductGuid();

            var basketGuid = _transactionClient.CreateBasket(currency, cultureCode, ct).Result;
            _contextHelper.SetCurrentCart(basketGuid);





            //TODO: Add to basket or create new basket
            //TODO: Must also handel if there is a cart allready
            //TODO: add currentproduct to cart

            //After the product has been added, show the product again.
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Uses the ContextHelper to find current product
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        private ProductViewModel CreateProductViewModel(CancellationToken ct)
        {
            var currentproductGuid = _contextHelper.GetCurrentProductGuid();
            var currentprodcutDto = _wildBearApiClient.GetSingleProductByGuid((Guid)currentproductGuid, ct);

            var productViewModel = new ProductViewModel()
            {
                Name = currentprodcutDto.Name,
                ShortDescription = currentprodcutDto?.ShortDescription,
                Price = currentprodcutDto.UnitPrices.FirstOrDefault().Value,
            };
            return productViewModel;
        }
    }
}
