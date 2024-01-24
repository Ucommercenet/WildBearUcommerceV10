using Microsoft.AspNetCore.Mvc;
using WildBearAdventures.MVC.Models;
using WildBearAdventures.MVC.WildBear.Context;
using WildBearAdventures.MVC.WildBear.TransactionApi;


namespace WildBearAdventures.MVC.Controllers
{
    //Handout Part 7 Transaction Endpoints
    #region Handout
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
                    total = orderline.total

                };
                shoppingCartViewModel.ShoppingChartOrderLineViewModels.Add(orderLineViewModel);
            }

            //TODO: Fix hardcoded value
            shoppingCartViewModel.ShoppingCartOrderTotal = 8000;

            return View(shoppingCartViewModel);
        }
    } 
    #endregion
}
