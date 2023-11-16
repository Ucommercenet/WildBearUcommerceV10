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

        public IActionResult Index(Guid productToAdd, CancellationToken token)
        {

            //TODO: Show Basket Details






            return View();
        }
    }
}
