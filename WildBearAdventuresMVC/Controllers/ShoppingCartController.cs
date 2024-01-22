using Microsoft.AspNetCore.Mvc;
using WildBearAdventuresMVC.Models;
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

            var shoppingCartDto = _transactionClient.GetShoppingCart((Guid)CurrentCart, ct).Result;

            var shoppingCartViewModel = new ShoppingCartViewModel()
            {
                ShoppingChartOrderLineViewModels = new List<OrderLineViewModel>()
            };

            foreach (var orderline in shoppingCartDto.orderLines)
            {
                var orderLineViewModel = new OrderLineViewModel()
                {
                    productName = orderline.productName,
                    quantity = orderline.quantity,
                    price = orderline.price,
                    //total = orderline.total
                    total = 5000
                };
                shoppingCartViewModel.ShoppingChartOrderLineViewModels.Add(orderLineViewModel);
            }



            return View(shoppingCartViewModel);
        }
    }
}
