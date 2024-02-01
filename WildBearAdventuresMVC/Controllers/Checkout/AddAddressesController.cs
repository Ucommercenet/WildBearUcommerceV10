using Microsoft.AspNetCore.Mvc;
using WildBearAdventures.MVC.WildBear.Models.DTOs;
using WildBearAdventures.MVC.WildBear.Models.Request;
using WildBearAdventures.MVC.WildBear.TransactionApi;
using static WildBearAdventures.MVC.WildBear.Models.Request.ShippingInformationRequest;


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

            var selectedCultureCode = "da-DK"; //TODO Improvement: Get from ContextHelper or user                                              
            var selectedPriceGroupName = "EUR 15 pct"; //TODO Improvement: Get from ContextHelper or user     
            var selectedPaymentMethodName = "Account"; //TODO Improvement: Get from User 



            var paymentMethodGuid = await FindPaymentMethodGuid(selectedCultureCode, selectedPaymentMethodName, ct);


            //Find PriceGroupId
            var priceGroups = await _transactionClient.GetPriceGroups(selectedCultureCode, ct);
            var selectedPriceGroupId = priceGroups.PriceGroups.Single(x => x.Name == selectedPriceGroupName).Id;

            //Find ShippingMethodId
            var shippingMethods = await _transactionClient.GetShippingMethods("","", "", ct );


            //Step 1 Add Shipment
            var shippingInformationRequest = new ShippingInformationRequest
            {
                ShoppingCartGuid = cartId,
                CultureCode = selectedCultureCode,
                PriceGroupId = selectedPriceGroupId,
                ShippingMethodId = "",
                ShippingAddress = new Address
                {
                    City = "Aarhus",
                    CompanyName = "Ucommerce",
                    CountryId = "TODO",
                    Email = "Test@notrealmail.com",
                    FirstName = "Sven",
                    LastName = "Splitbeard",
                    Line1 = "Klostergade 21",
                    Line2 = "",
                    MobileNumber = "",
                    PhoneNumber = "",
                    PostalCode = "8000",
                    State = ""
                }
            };


            _transactionClient.AddShippingInformation(shippingInformationRequest, ct);


            return View();
        }


    }
}
