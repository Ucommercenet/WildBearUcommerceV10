using Microsoft.AspNetCore.Mvc;
using WildBear.Shop.Coffee.WildBear.MockData;
using WildBear.Shop.Coffee.WildBear.Models.DTOs;
using WildBear.Shop.Coffee.WildBear.Models.Request;
using WildBear.Shop.Coffee.WildBear.TransactionApi;
using WildBearAdventures.MVC.WildBear.Models.Request;
using static WildBear.Shop.Coffee.WildBear.Models.DTOs.ShippingMethodCollectionDto;
using static WildBear.Shop.Coffee.WildBear.Models.Request.ShippingInformationRequest;


namespace WildBear.Shop.Coffee.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly TransactionClient _transactionClient;

        public CheckoutController(TransactionClient transactionClient)
        {
            _transactionClient = transactionClient;
        }

        public async Task<IActionResult> Index(Guid cartId, CancellationToken ct)
        {
            var selectedCultureCode = "da-DK"; //TODO Improvement: Get from ContextHelper or user                                              
            var selectedPriceGroupName = "EUR 15 pct"; //TODO Improvement: Get from ContextHelper or user     
            var selectedPaymentMethodName = "Account"; //TODO Improvement: Get from User 
            var selectedShippingMethod = "Download"; //TODO Improvement: Get from User 

            //Handout Part 6: Checkout flow

            //Find PriceGroupId
            var priceGroups = await _transactionClient.GetPriceGroups(selectedCultureCode, ct);
            var selectedPriceGroupId = priceGroups.PriceGroups.Single(x => x.Name == selectedPriceGroupName).Id;

            var countriesDto = await _transactionClient.GetCountries(ct);
            var selectedCountry = countriesDto.Countries.First();


            var shippingInformation = await AddShippingInfoToCart(cartId, selectedCountry, selectedCultureCode, selectedPriceGroupId, selectedShippingMethod, ct);

            var isShippingInfoAlsoBillingInfo = true;

            await AddBillingInfoToCart(cartId, isShippingInfoAlsoBillingInfo, shippingInformation, ct);

            var paymentResponseDto = await CheckOutCart(cartId, selectedCultureCode, selectedPaymentMethodName, priceGroups, selectedPriceGroupName, ct);


            //Note: The PaymentUrl when using the defaultPaymentService both the takeControl redirect and the callback
            return Redirect(paymentResponseDto.PaymentUrl);
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

        private async Task<PaymentResponseDto> CheckOutCart(Guid cartId, string selectedCultureCode, string selectedPaymentMethodName, PriceGroupCollectionDto priceGroups, string selectedPriceGroupName, CancellationToken ct)
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

            //Payment
            var paymentResponseDto = await _transactionClient.PostCreatePayment(createPaymentRequest, ct);

            return paymentResponseDto;
        }

        private async Task<ShippingInformationRequest> AddShippingInfoToCart(Guid cartId, Country selectedCountry, string selectedCultureCode, string selectedPriceGroupId, string selectedShippingMethod, CancellationToken ct)
        {
            var shippingMethods = await _transactionClient.GetShippingMethods(selectedCountry.Id, selectedCultureCode, selectedPriceGroupId, ct);
            var selectedShippingMethodId = shippingMethods.ShippingMethods.First(x => x.Name == selectedShippingMethod).Id;
            var mockAddress = MockFictionalDataGenerator.GetRandomAddress();


            //Step 1 Add Shipment
            var shippingInformationRequest = new ShippingInformationRequest
            {
                ShoppingCartGuid = cartId,
                CultureCode = selectedCultureCode,
                PriceGroupId = selectedPriceGroupId,
                ShippingMethodId = selectedShippingMethodId,
                ShippingAddress = new Address
                {
                    City = mockAddress.City,
                    CompanyName = mockAddress.CompanyName,
                    CountryId = selectedCountry.Id,
                    Email = mockAddress.Email,
                    FirstName = mockAddress.FirstName,
                    LastName = mockAddress.LastName,
                    Line1 = mockAddress.Line1,
                    Line2 = mockAddress.Line2,
                    MobileNumber = mockAddress.MobileNumber,
                    PhoneNumber = mockAddress.PhoneNumber,
                    PostalCode = mockAddress.PostalCode,
                    State = mockAddress.State
                }
            };

            //Shipping Information
            await _transactionClient.PostCartShippingInformation(shippingInformationRequest, ct);

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
