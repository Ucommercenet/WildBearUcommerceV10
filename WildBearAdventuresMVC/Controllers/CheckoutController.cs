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



            var paymentMethodId = await FindPaymentMethodId(selectedCultureCode, selectedPaymentMethodName, ct);

            var priceGroups = await _transactionClient.GetPriceGroups(selectedCultureCode, ct);
            var priceGroup = priceGroups.priceGroups.Single(x => x.name == selectedPriceGroupName);

            var createPaymentRequest = new CreatePaymentRequest()
            {
                CultureCode = selectedCultureCode,
                PaymentMethodId = Guid.NewGuid(),
                PriceGroupGuid = new Guid(priceGroup.id),
                ShoppingCartId = cartId,


            };

            _transactionClient.PostCreatePayment(createPaymentRequest, ct);


            //TODO: Call with hadcode data

            //TODO: See if Payment works

            //TODO: Figure out how Ucommerce Payment redicrec works



            return View();
        }

        private async Task<Guid> FindPaymentMethodId(string selectedCultureCode, string selectedPaymentMethodName, CancellationToken ct)
        {
            var countriesDto = await _transactionClient.GetCountries(ct);

            //TODO: Get specific From billing info
            var billingCountry =  countriesDto.countries.First(x => x.cultureCode == selectedCultureCode);

            //TODO: Need CountryId and CultureCode
            var paymentMethods = await _transactionClient.DRAFT_GetPaymentMethods(billingCountry.id, selectedCultureCode, ct);

           

            return Guid.NewGuid();
        }
    }
}
