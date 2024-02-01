using Microsoft.AspNetCore.Mvc;
using WildBearAdventures.MVC.ViewModels;
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


        //Handout Part 5 Shopping Cart
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var CurrentCart = _contextHelper.GetCurrentCartGuid();
            //TODO Improvement: Handle if no cart is found
            var shoppingCartDto = await _transactionClient.GetShoppingCart((Guid)CurrentCart, ct);

            var shoppingCartViewModel = new ShoppingCartViewModel()
            {
                ShoppingChartOrderLineViewModels = new List<OrderLineViewModel>(),
                ShoppingCartOrderTotal = shoppingCartDto.orderTotal,
                ShoppingCartGuid = (Guid)CurrentCart
            };

            foreach (var orderLine in shoppingCartDto.orderLines)
            {
                var orderLineViewModel = new OrderLineViewModel()
                {
                    productName = orderLine.productName,
                    quantity = orderLine.quantity,
                    price = orderLine.price,
                    total = orderLine.total

                };
                shoppingCartViewModel.ShoppingChartOrderLineViewModels.Add(orderLineViewModel);
            }

            
            

            return View(shoppingCartViewModel);
        }
    } 
    #endregion
}
