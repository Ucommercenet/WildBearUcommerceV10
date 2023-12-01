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

        public IActionResult Index(CancellationToken token)
        {

            //TODO: Show Basket Details - Load up Cart.Lines 


            //var ShoppingCartViewModel = new Sh



            return View();
        }
    }
}
