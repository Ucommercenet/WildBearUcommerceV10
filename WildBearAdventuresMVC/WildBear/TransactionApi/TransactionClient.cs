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

    public async Task<PriceGroupCollectionDto> GetPriceGroups(string cultureCode, CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        var response = await client.GetAsync(requestUri: $"/api/v1/price-groups?cultureCode={cultureCode}");
        var priceGroupsDto = await response.Content.ReadAsAsync<PriceGroupCollectionDto>();


        return priceGroupsDto;
    }

    public async Task<PaymentMethodsDto> GetPaymentMethods(string countryId, string selectedCultureCode, CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        var response = await client.GetAsync(requestUri: $"/api/v1/payment-methods?cultureCode={selectedCultureCode}&countryId={countryId}");

        var paymentMethodsDto = await response.Content.ReadAsAsync<PaymentMethodsDto>();


        return paymentMethodsDto;
    }

    public async Task<CountryCollectionDto> GetCountries(CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        var response = await client.GetAsync(requestUri: $"/api/v1/countries");

        var countriesDto = await response.Content.ReadAsAsync<CountryCollectionDto>();

        return countriesDto;
    }

    public async Task<ShippingMethodCollectionDto> GetShippingMethods(string countryId, string selectedCultureCode, Guid priceGroupId, CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        var response = await client.GetAsync($"/api/v1/shipping-methods?countryId={countryId}&cultureCode={selectedCultureCode}&priceGroupId={priceGroupId}", ct);

        response.EnsureSuccessStatusCode(); // Ensure the response is successful, you can handle errors based on your requirements

        var shippingMethodsDto = await response.Content.ReadAsAsync<ShippingMethodCollectionDto>();

        return shippingMethodsDto;
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
            { "cartId", $"{createPaymentRequest.ShoppingCartGuid}" },
            { "cultureCode", $"{createPaymentRequest.CultureCode}"  },
            { "paymentMethodId", $"{createPaymentRequest.PaymentMethodGuid}" },
            { "priceGroupId", $"{createPaymentRequest.PriceGroupGuid}" },
        };

        var createPaymentResponse = await client.PostAsJsonAsync("/api/v1/payments", requestPayload);



        var content = await createPaymentResponse.Content.ReadAsStringAsync();

        if (createPaymentResponse.IsSuccessStatusCode is false)
        { throw new Exception($"Could not create payment"); }

        return;
    }

    public async Task PostShoppingCartLineUpdate(ShoppingCartLineUpdateRequest request, CancellationToken ct)
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

    public async Task<string> PostCartShippingInformation(ShippingInformationRequest shippingInformationRequest, CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        Dictionary<string, object> requestPayload = new()
        {
            { "PriceGroupId", shippingInformationRequest.PriceGroupId},
            { "shippingMethodId", shippingInformationRequest.ShippingMethodId },
            { "cultureCode", shippingInformationRequest.CultureCode },
            { "shippingAddress", shippingInformationRequest.ShippingAddress },
            
        };

        var response = await client.PostAsJsonAsync(requestUri: $"/api/v1/carts/{shippingInformationRequest.ShoppingCartGuid}/shipping", value: requestPayload);

        var humanReadableResponseInJson = JToken.Parse(response.Content.ReadAsStringAsync().Result).ToString();


        throw new NotImplementedException();
    }

    //Only use this for scaffolding new API Calls
    public async Task<string> GetDRAFT(CancellationToken ct)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(ct);

        Dictionary<string, object> requestPayload = new()
        {
            //Required
            { "Quantity", "Todo"},
            { "Sku", "Todo" },
            { "CultureCode", "Todo" },
            { "PriceGroupId", "Todo" },
            { "CatalogId", "Todo" }, //Must be named CatalogId not productCatalogId
        };

        var response = await client.GetAsync(requestUri: $"/api/v1/payment-methods");

        var humanReadableResponseInJson = JToken.Parse(response.Content.ReadAsStringAsync().Result).ToString();


        throw new NotImplementedException();
    }

 
}
