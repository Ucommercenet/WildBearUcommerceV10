using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Identity;
using Newtonsoft.Json.Linq;
using System.Threading;
using WildBearAdventures.MVC.WildBear.Models.DTOs;
using WildBearAdventures.MVC.WildBear.Models.Request;

namespace WildBearAdventures.MVC.WildBear.TransactionApi;

public class TransactionClient
{

    private readonly StoreAuthorization _storeAuthorizationFlow;

    public TransactionClient(StoreAuthorization storeAuthorization)
    {
        _storeAuthorizationFlow = storeAuthorization;
    }

    #region API GET Calls
    public async Task<ShoppingCartDto> GetShoppingCart(Guid shoppingCartGuid, CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        var response = await client.GetAsync(requestUri: $"/api/v1/carts/{shoppingCartGuid}");

        var shoppingCart = response.Content.ReadAsAsync<ShoppingCartDto>().Result;

        return shoppingCart;

    }


    public async Task<PriceGroupsDto> GetPriceGroups(string cultureCode, CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        var response = await client.GetAsync(requestUri: $"/api/v1/price-groups?cultureCode={cultureCode}");               
        var priceGroupsDto = await response.Content.ReadAsAsync<PriceGroupsDto>();
               

        return priceGroupsDto;
    }

    public async Task<string> DRAFT_GetPaymentMethods(string countryId, string selectedCultureCode, CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        var response = await client.GetAsync(requestUri: $"/api/v1/payment-methods?cultureCode={selectedCultureCode}&countryId={countryId}");

        var humanReadableJson = JToken.Parse(response.Content.ReadAsStringAsync().Result).ToString();


        throw new NotImplementedException();
    }

    public async Task<CountriesDto> GetCountries(CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        var response = await client.GetAsync(requestUri: $"/api/v1/countries");

        var countriesDto = await response.Content.ReadAsAsync<CountriesDto>();

        return countriesDto;
    }

    #endregion

    #region API POST Calls
    /// <summary>
    /// 
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="cultureCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The guid of the basket just created</returns>
    /// <exception cref="Exception"></exception>
    public async Task<Guid> PostCreateBasket(string currency, string cultureCode, CancellationToken cancellationToken)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(cancellationToken);

        var requestPayload = new Dictionary<string, string>
        {
            //Required
            {"currency", currency },
            {"cultureCode", cultureCode }
        };
        var createBasketResponse = await client.PostAsJsonAsync("/api/v1/baskets", requestPayload);

        if (createBasketResponse.IsSuccessStatusCode is false)
        { throw new Exception($"Could not create new Basket"); }

        var responseResult = await createBasketResponse.Content.ReadAsAsync<CreateShoppingCartDto>();

        return responseResult.BasketId;
    }

    public async Task PostCreatePayment(CreatePaymentRequest createPaymentRequest, CancellationToken cancellationToken)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(cancellationToken);

        var requestPayload = new Dictionary<string, string>
        {
            { "cartId", $"{createPaymentRequest.ShoppingCartId}" },
            { "cultureCode", $"{createPaymentRequest.CultureCode}"  },
            { "paymentMethodId", $"{createPaymentRequest.PaymentMethodId}" },
            { "priceGroupId", $"{createPaymentRequest.PriceGroupGuid}" },
        };

        var createPaymentResponse = await client.PostAsJsonAsync("/api/v1/payments", requestPayload);



        var content = createPaymentResponse.Content.ReadAsStringAsync().Result;

        if (createPaymentResponse.IsSuccessStatusCode is false)
        { throw new Exception($"Could not create payment"); }

        return;
    }

    public async Task PostShoppingCartLine(ShoppingCartLineUpdateRequest request, CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        Dictionary<string, object> requestPayload = new()
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

        var content = response.Content.ReadAsStringAsync().Result;

        if (response.IsSuccessStatusCode is false)
        { throw new Exception($"Could not UpdateOrderLineQuantity"); }

        return;
    }
    #endregion

    //Only use this for scaffolding new API Calls
    public async Task<string> GetDRAFT(CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        var response = await client.GetAsync(requestUri: $"/api/v1/payment-methods");        

        var humanReadableJson = JToken.Parse(response.Content.ReadAsStringAsync().Result).ToString();


        throw new NotImplementedException();
    }

}
