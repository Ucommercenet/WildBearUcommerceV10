using Microsoft.AspNetCore.Mvc;
using WildBearAdventures.MVC.WildBear.Models.Request;
using WildBearAdventures.MVC.WildBear.TransactionApi;


namespace WildBearAdventures.MVC.Controllers.Checkout
{
    public class AddAddressesController : Controller
    {
        private readonly TransactionClient _transactionClient;

        public AddAddressesController(TransactionClient transactionClient)
        {
            _transactionClient = transactionClient;
        }

        public async Task<IActionResult> Index(Guid cartId, CancellationToken ct)
        {


            return View();
        }


    }
}
