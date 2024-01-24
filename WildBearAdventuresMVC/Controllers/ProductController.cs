using Microsoft.AspNetCore.Mvc;
using WildBearAdventures.MVC.Models;
using WildBearAdventures.MVC.WildBear.Context;
using WildBearAdventures.MVC.WildBear.Models.Request;
using WildBearAdventures.MVC.WildBear.TransactionApi;



namespace WildBearAdventures.MVC.Controllers
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
        public async Task<RedirectToActionResult> AddToCart(Guid? productGuid, CancellationToken ct, int quantity = 1)
        {
            var currency = "EUR"; //TODO: Get dynamic
            var cultureCode = "da-DK"; //TODO: Get dynamic


            var currentCategory = _contextHelper.GetCurrentCategoryGuid() ?? throw new Exception("No Category found");
            var currentCatalog = _wildBearApiClient.GetSingleCategoryByGuid(currentCategory, ct).CatalogId;

            //Note: does also use the _transactionClient
            var basketGuid = FindCurrentShoppingCartOrCreateNew(currency, cultureCode, ct);
            _contextHelper.SetCurrentCart(basketGuid);

            //CurrentProduct
            var currentProductGuid = (productGuid ?? _contextHelper.GetCurrentProductGuid()) ?? throw new Exception("No product found");
            var product = _wildBearApiClient.GetSingleProductByGuid(currentProductGuid, ct);
            var priceGroupGuid = product.PriceGroupIds.First();


            var request = new ShoppingCartLineUpdateRequest
            {
                ShoppingCart = basketGuid,
                CultureCode = cultureCode,
                Quantity = quantity,
                PriceGroupGuid = new Guid(priceGroupGuid),
                Catalog = new Guid(currentCatalog),
                Sku = product.Sku,
                VariantSku = product.VariantSku
            };

            await _transactionClient.PostShoppingCartLine(request, ct);


            _contextHelper.UpdateCurrentShoppingCartCount(quantity);
            //After the product has been added, show the product again.
            return RedirectToAction("Index");
        }

        private Guid FindCurrentShoppingCartOrCreateNew(string currency, string cultureCode, CancellationToken ct)
        {
            var currentBasketGuid = _contextHelper.GetCurrentCartGuid();

            //currentBasket did exists
            if (currentBasketGuid.HasValue is true)
            { return (Guid)currentBasketGuid; }

            //currentBasket did not exists
            var basketGuid = _transactionClient.PostCreateBasket(currency, cultureCode, ct).Result;
            _contextHelper.SetCurrentCart(basketGuid);
            return basketGuid;

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
                        
            currentprodcutDto.UnitPrices.TryGetValue("EUR 15 pct", out var price);

            var productViewModel = new ProductViewModel()
            {
                Name = currentprodcutDto.Name,
                ShortDescription = currentprodcutDto?.ShortDescription,
                Price = price            
            };




            return productViewModel;
        }




    }
}
