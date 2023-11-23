using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models;
using WildBearAdventuresMVC.WildBear.Interfaces;
using WildBearAdventuresMVC.WildBear.TransactionApi;
using WildBearAdventuresMVC.WildBear.TransactionApi.Models;

namespace WildBearAdventuresMVC.Controllers
{

    public class ProductController : Controller
    {
        private readonly IStoreApiClient _wildBearApiClient;
        private readonly IContextHelper _contextHelper;
        private readonly TransactionClient _transactionClient;

        public ProductController(IStoreApiClient wildBearApiClient, IContextHelper contextHelper, TransactionClient transactionClient)
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
        public async Task<RedirectToActionResult> AddToCart(Guid? productGuid, CancellationToken ct)
        {
            var currency = "EUR";
            var cultureCode = "da-DK";

            var priceGroupGuid = new Guid("8769e717-08d2-4313-82a9-30d4f4886663"); //EUR 15 pct //Optimize: Get from endpoint
            var catalog = new Guid("1e2b7c56-9ebd-4443-86ab-6224105836ad"); //MainProductCatalog //TODO: Get this somehow

            var currentproduct = productGuid ?? _contextHelper.GetCurrentProductGuid();

            var basketGuid = _transactionClient.CreateBasket(currency, cultureCode, ct).Result;
            _contextHelper.SetCurrentCart(basketGuid);


            //TODO: Get sku and VariantSKu via GetproductByGuid


            var request = new UpdateOrderLineQuantityRequest
            {
                ShoppingCart = basketGuid, //OK from CreateBasket endpoint
                CultureCode = cultureCode, //OK same as endpoint               
                Quantity = 1,
                PriceGroupGuid = priceGroupGuid, //OK 
                Catalog = catalog, //OK
                Sku = "A001",
                VariantSku = "AS1"      //What is a good way of getting Sku dynamic?
            };

            await _transactionClient.UpdateOrderLineQuantity(request, ct);


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
