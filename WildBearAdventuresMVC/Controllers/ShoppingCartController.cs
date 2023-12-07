using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.WildBear.Interfaces;
using WildBearAdventuresMVC.WildBear.TransactionApi;

namespace WildBearAdventuresMVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IContextHelper _contextHelper;
        private readonly TransactionClient _transactionClient;


        public ShoppingCartController(IContextHelper contextHelper, TransactionClient transactionClient)
        {
            _contextHelper = contextHelper;
            _transactionClient = transactionClient;
        }

        public IActionResult Index(CancellationToken ct)
        {

            //TODO: STEPS: Get Current Cart , use GetCart on Transaction client

            var CurrentCart = _contextHelper.GetCurrentCartGuid();

            var DRAFT_result = _transactionClient.GetShoppingCart((Guid)CurrentCart, ct).Result;



            return View();
        }
    }
}
