using Microsoft.AspNetCore.Mvc;
using WildBearAdventures.MVC.WildBear.Models.Request;
using WildBearAdventures.MVC.WildBear.TransactionApi;


namespace WildBearAdventures.MVC.Controllers
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



            var paymentMethodGuid = await FindPaymentMethodGuid(selectedCultureCode, selectedPaymentMethodName, ct);

            var priceGroups = await _transactionClient.GetPriceGroups(selectedCultureCode, ct);
            var priceGroup = priceGroups.PriceGroups.Single(x => x.Name == selectedPriceGroupName);

            var createPaymentRequest = new CreatePaymentRequest()
            {
                CultureCode = selectedCultureCode,
                PaymentMethodGuid = paymentMethodGuid,
                PriceGroupGuid = new Guid(priceGroup.Id),
                ShoppingCartGuid = cartId,


            };

            _transactionClient.PostCreatePayment(createPaymentRequest, ct);


            //TODO: Call with hadcode data

            //TODO: See if Payment works

            //TODO: Figure out how Ucommerce Payment redicrec works



            return View();
        }

        private async Task<Guid> FindPaymentMethodGuid(string selectedCultureCode, string selectedPaymentMethodName, CancellationToken ct)
        {
            var countriesDto = await _transactionClient.GetCountries(ct);
            
            //TODO: Get specific From billing info
            var billingCountry =  countriesDto.Countries.First(x => x.CultureCode == selectedCultureCode);

            //TODO: Need CountryId and CultureCode
            var paymentMethods = await _transactionClient.GetPaymentMethods(billingCountry.Id, selectedCultureCode, ct);

            var selectedPaymentMethod = paymentMethods.PaymentMethods.FirstOrDefault(x => x.Name == selectedPaymentMethodName);

            return new Guid(selectedPaymentMethod.Id) ;
        }
    }
}
