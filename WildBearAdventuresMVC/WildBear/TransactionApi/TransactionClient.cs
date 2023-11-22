using WildBearAdventuresMVC.WildBear.TransactionApi.Models;

namespace WildBearAdventuresMVC.WildBear.TransactionApi
{
    public class TransactionClient
    {

        private readonly StoreAuthorizationFlow _storeAuthorizationFlow;

        public TransactionClient(StoreAuthorizationFlow storeAuthorizationFlow)
        {
            _storeAuthorizationFlow = storeAuthorizationFlow;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="cultureCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The guid of the basket just created</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Guid> CreateBasket(string currency, string cultureCode, CancellationToken cancellationToken)
        {
            using var client = _storeAuthorizationFlow.GetTransactionReadyClient(cancellationToken);

            var payload = new Dictionary<string, string> { { "currency", currency }, { "cultureCode", cultureCode } };
            var createBasketResponse = await client.PostAsJsonAsync("/api/v1/baskets", payload);

            if (createBasketResponse.IsSuccessStatusCode is false)
            { throw new Exception($"Couldn't create new Basket"); }

            var output = await createBasketResponse.Content.ReadAsAsync<ApiOutputCreateBasket>();

            return output.BasketId;
        }

        //public async string GetBasket(Guid basketGuid, CancellationToken cancellationToken)
        //{
        //    using HttpClient client = GetTransactionReadyClient(cancellationToken);



        //    var BasketResponse = await client.GetAsync($"/api/v1/baskets/{basketGuid}");


        //    return "todo";
        //}













    }

}
