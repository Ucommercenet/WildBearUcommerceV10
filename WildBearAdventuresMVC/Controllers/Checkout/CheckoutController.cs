using Microsoft.AspNetCore.Mvc;
using WildBearAdventures.MVC.WildBear.Models.DTOs;
using WildBearAdventures.MVC.WildBear.Models.Request;
using WildBearAdventures.MVC.WildBear.TransactionApi;
using static WildBearAdventures.MVC.WildBear.Models.DTOs.ShippingMethodCollectionDto;
using static WildBearAdventures.MVC.WildBear.Models.Request.ShippingInformationRequest;


namespace WildBearAdventures.MVC.Controllers.Checkout
{
    public class CheckoutController : Controller
    {
        private readonly TransactionClient _transactionClient;

        public CheckoutController(TransactionClient transactionClient)
        {
            _transactionClient = transactionClient;
        }
        
        //Handout Checkout Shopping Cart. For now let's do it all in a single Controller and split out afterward

        public async Task<IActionResult> Index(Guid cartId, CancellationToken ct)
        {

            var selectedCultureCode = "da-DK"; //TODO Improvement: Get from ContextHelper or user                                              
            var selectedPriceGroupName = "EUR 15 pct"; //TODO Improvement: Get from ContextHelper or user     
            var selectedPaymentMethodName = "Account"; //TODO Improvement: Get from User 
            var selectedShippingMethod = "Standard Shipping"; //TODO Improvement: Get from User 

            //Find PriceGroupId
            var priceGroups = await _transactionClient.GetPriceGroups(selectedCultureCode, ct);
            var selectedPriceGroupId = priceGroups.PriceGroups.Single(x => x.Name == selectedPriceGroupName).Id;

            var countriesDto = await _transactionClient.GetCountries(ct);
            var selectedCountry = countriesDto.Countries.First();

            
            var shippingInfomation = await AddShippingInfoToCart(cartId, selectedCountry, selectedCultureCode, selectedPriceGroupId, selectedShippingMethod, ct);

            var isShippingInfoAlsoBillingInfo = true;
            
            await AddBillingInfoToCart(cartId, isShippingInfoAlsoBillingInfo, shippingInfomation, ct);

            await CheckOutCart(cartId, selectedCultureCode, selectedPaymentMethodName, priceGroups, selectedPriceGroupName, ct);


            return View();
        }

        private async Task AddBillingInfoToCart(Guid cartId, bool isShippingInfoAlsoBillingInfo, ShippingInformationRequest shippingInfomation, CancellationToken ct)
        {

            //For teaching purposes we assume it's the same address for shipping and for billing
            if (isShippingInfoAlsoBillingInfo is false)
            {
                throw new NotImplementedException();
            }

            var billingAddressRequest = new BillingAddressRequest
            {
                ShoppingCartGuid = cartId,
                City = shippingInfomation.ShippingAddress.City,
                FirstName = shippingInfomation.ShippingAddress.FirstName,
                LastName = shippingInfomation.ShippingAddress.LastName,
                PostalCode = shippingInfomation.ShippingAddress.PostalCode,
                Line1 = shippingInfomation.ShippingAddress.Line1,
                CountryId = shippingInfomation.ShippingAddress.CountryId,
                Email = shippingInfomation.ShippingAddress.Email,
                State = shippingInfomation.ShippingAddress.State,
                MobileNumber = shippingInfomation.ShippingAddress.MobileNumber,
                Attention = null, //Optional
                CompanyName = null, //Optional  
            };

            await _transactionClient.PostCartBillingInfomation(billingAddressRequest, ct);
        }

        private async Task CheckOutCart(Guid cartId, string selectedCultureCode, string selectedPaymentMethodName, PriceGroupCollectionDto priceGroups, string selectedPriceGroupName, CancellationToken ct)
        {
            var paymentMethodGuid = await FindPaymentMethodGuid(selectedCultureCode, selectedPaymentMethodName, ct);
            var priceGroup = priceGroups.PriceGroups.Single(x => x.Name == selectedPriceGroupName);

            var createPaymentRequest = new CreatePaymentRequest()
            {
                CultureCode = selectedCultureCode,
                PaymentMethodGuid = paymentMethodGuid,
                PriceGroupGuid = new Guid(priceGroup.Id),
                ShoppingCartGuid = cartId,


            };

            //TODO: will it work without billing information?

            //Payment
            _transactionClient.PostCreatePayment(createPaymentRequest, ct);
        }

        private async Task<ShippingInformationRequest> AddShippingInfoToCart(Guid cartId, Country selectedCountry, string selectedCultureCode, string selectedPriceGroupId, string selectedShippingMethod, CancellationToken ct)
        {
            var shippingMethods = await _transactionClient.GetShippingMethods(selectedCountry.Id, selectedCultureCode, selectedPriceGroupId, ct );
            var selectedShippingMethodId = shippingMethods.ShippingMethods.First(x => x.Name == selectedShippingMethod).Id;

            //Step 1 Add Shipment
            var shippingInformationRequest = new ShippingInformationRequest
            {
                ShoppingCartGuid = cartId,
                CultureCode = selectedCultureCode,
                PriceGroupId = selectedPriceGroupId,
                ShippingMethodId = selectedShippingMethodId,
                ShippingAddress = new Address
                {
                    City = "Aarhus",
                    CompanyName = "Ucommerce",
                    CountryId = selectedCountry.Id,
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
            
            //Shipping Information
            await _transactionClient.PostCartShippingInformation(shippingInformationRequest, ct);
            
            //The billing information might need to be the same.  
            return shippingInformationRequest;
        }

        private async Task<Guid> FindPaymentMethodGuid(string selectedCultureCode, string selectedPaymentMethodName, CancellationToken ct)
        {
            var countriesDto = await _transactionClient.GetCountries(ct);

            //TODO: Get specific From billing info
            var billingCountry = countriesDto.Countries.First(x => x.CultureCode == selectedCultureCode);

            //TODO: Need CountryId and CultureCode
            var paymentMethods = await _transactionClient.GetPaymentMethods(billingCountry.Id, selectedCultureCode, ct);

            var selectedPaymentMethod = paymentMethods.PaymentMethods.FirstOrDefault(x => x.Name == selectedPaymentMethodName);

            return new Guid(selectedPaymentMethod.Id);
        }

    }
}
