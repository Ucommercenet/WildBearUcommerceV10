using Microsoft.AspNetCore.Mvc;
using WildBear.Shop.Coffee.WildBear.TransactionApi;
using WildBearAdventures.MVC.WildBear.Models.DTOs;

namespace WildBear.Shop.Coffee.Controllers
{
    public class DiscoverPipelinesController : Controller
    {
        private readonly StoreAuthorization _storeAuthorizationFlow;

        public DiscoverPipelinesController(StoreAuthorization storeAuthorizationFlow)
        {
            _storeAuthorizationFlow = storeAuthorizationFlow;
        }

        public IActionResult Index(CancellationToken ct)
        {


            GetAllPipelines(ct);



            return View();
        }

        private void GetAllPipelines(CancellationToken ct)
        {
            using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);
            var uri = $"/ucommerce/api/v1.0/pipelines";

            var response = client.GetAsync(uri, ct).Result;
            var result = response.Content.ReadAsStringAsync().Result;

        }
    }
}
