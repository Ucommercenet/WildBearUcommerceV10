using Microsoft.AspNetCore.Mvc;
using WildBear.Shop.Coffee.ViewModels;
using WildBear.Shop.Coffee.WildBear.Context;
using WildBear.Shop.Coffee.WildBear.TransactionApi;

namespace WildBear.Shop.Coffee.Controllers
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

}
