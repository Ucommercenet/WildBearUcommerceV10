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

            var basketGuid = _transactionClient.CreateBasket("Todo", "todo", token);


            return View();
        }
    }
}
