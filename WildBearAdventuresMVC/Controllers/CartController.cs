using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.WildBear.Interfaces;
using WildBearAdventuresMVC.WildBear.TransactionApi;

namespace WildBearAdventuresMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly IContextHelper _contextHelper;
        private readonly ITransactionClient _transactionClient;


        public CartController(IContextHelper contextHelper, ITransactionClient transactionClient)
        {
            _contextHelper = contextHelper;
            _transactionClient = transactionClient;
        }

        public IActionResult Index(CancellationToken token)
        {
            //TODO: AddToCart (if no Cart call CreateCart)

            var currency = "DKK";
            var cultureCode = "en-DK";

            var currentproduct = _contextHelper.GetCurrentProductGuid();

            var basketGuid = _transactionClient.CreateBasket(currency, cultureCode, token).Result;


            return View();
        }
    }
}
