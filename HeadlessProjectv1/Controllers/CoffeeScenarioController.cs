using Microsoft.AspNetCore.Mvc;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;

namespace HeadlessProjectv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeScenarioController : ControllerBase
    {
        //Easy Coffee scenario: This is anti pattern! But lets make it work then move the controllers out of this class

        [HttpGet("GetCoffeeProduct")]
        public ProductSearchModel GetCoffeeProduct()
        {
            throw new NotImplementedException();
        }

        [HttpPost("AddProductToCart")]
        public ProductSearchModel AddProductToCart()
        {
            throw new NotImplementedException();
        }

        [HttpPost("CheckoutCart")]
        public ProductSearchModel CheckoutCart()
        {
            throw new NotImplementedException();
        }

        [HttpPost("GetOrders")]
        public ProductSearchModel GetOrders()
        {
            throw new NotImplementedException();
        }
    }
}