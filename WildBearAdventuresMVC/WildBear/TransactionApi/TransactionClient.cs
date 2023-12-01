using WildBearAdventuresMVC.WildBear.TransactionApi.Models;

namespace WildBearAdventuresMVC.WildBear.TransactionApi;

public class TransactionClient
{

    private readonly StoreAuthorization _storeAuthorizationFlow;

    public TransactionClient(StoreAuthorization storeAuthorization)
    {
        _storeAuthorizationFlow = storeAuthorization;
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

        var requestPayload = new Dictionary<string, string>
        {
            //Required
            {"currency", currency },
            {"cultureCode", cultureCode }
        };
        var createBasketResponse = await client.PostAsJsonAsync("/api/v1/baskets", requestPayload);

        if (createBasketResponse.IsSuccessStatusCode is false)
        { throw new Exception($"Could not create new Basket"); }

        var responseResult = await createBasketResponse.Content.ReadAsAsync<ApiOutputCreateBasket>();

        return responseResult.BasketId;
    }




    public async Task UpdateOrderLineQuantity(UpdateOrderLineQuantityRequest request, CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetTransactionReadyClient(ct);

        Dictionary<string, object> requestPayload = new Dictionary<string, object>
        {
            //Required
            { "Quantity", request.Quantity},
            { "Sku", request.Sku },
            { "CultureCode", request.CultureCode },
            { "PriceGroupId", request.PriceGroupGuid },
            { "CatalogId", request.Catalog }, //Must be named CatalogId not productCatalogId
        };

        //Optional
        if (request.VariantSku is not null) { requestPayload.Add("VariantSku", request.VariantSku); }

        var response = await client.PostAsJsonAsync(requestUri: $"/api/v1/carts/{request.ShoppingCart}/lines", value: requestPayload);

        var contentString = response.Content.ReadAsStringAsync().Result;





        if (response.IsSuccessStatusCode is false)
        { throw new Exception($"Could not UpdateOrderLineQuantity"); }

        return;
    }


}
