using WildBearAdventures.MVC.WildBear.Models.DTOs;
using WildBearAdventures.MVC.WildBear.TransactionApi;

namespace WildBearAdventures.MVC.WildBear.WildBearApi;



public class WildBearApiClient : IStoreApiClient
{
    private readonly StoreAuthorization _storeAuthorizationFlow;

    public WildBearApiClient(StoreAuthorization storeAuthorizationFlow)
    {
        _storeAuthorizationFlow = storeAuthorizationFlow;
    }

    #region Product Related
    public ProductDto GetRandomProductFromCategory(Guid categoryGuid, CancellationToken token)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);

        var uri = $"/Product/GetAllProductsFromCategoryGuid?categoryId={categoryGuid}";
        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<List<ProductDto>>().Result;
        var randomIndex = new Random().Next(result.Count);

        return result[randomIndex];

    }
    public List<ProductDto> GetAllProductsFromCategoryGuid(Guid categoryGuid, CancellationToken token)
    {

        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);
        var uri = $"https://localhost:44381/api/Product/GetAllProductsFromCategoryGuid?categoryId={categoryGuid}";

        var response = client.GetAsync(uri, token).Result;

        //IMPROVE: Fix breaks if the there is zero products in the list
        //var result = response.Content.ReadFromJsonAsync<List<ProductDto?>>().Result;
        var allProducts = response.Content.ReadFromJsonAsync<List<ProductDto?>>().Result;

        //TODO: Also include non-variants

        var NotSellableProducts = allProducts.Where(x => x.AllowOrdering == false).ToList();

        var regularProduct = allProducts.Where(x => x.productType == 2 ).ToList();
        var variantProduct = allProducts.Where(x => x.productType == 3 ).ToList();


        var sellableProducts = allProducts.Where(x => x.productType == 2 | x.productType == 3 ).ToList();

        


        return sellableProducts;

    }
    public ProductDto GetSingleProductByGuid(Guid productGuid, CancellationToken token)
    {

        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);

        var uri = $"https://localhost:44381/api/Product/GetProductByGuid?searchGuid={productGuid}";

        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<ProductDto>().Result;


        return result;
    }

    public ProductDto GetSingleProductByName(string searchName, CancellationToken token)
    {

        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);

        var uri = $"https://localhost:44381/api/Product/GetProductByName?searchName={searchName}";

        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<ProductDto>().Result;


        return result;
    }


    #endregion


    #region Category Related

    //MasterClass first encounter with the Api client keep this as scaffolding   
    public List<CategoryDto> GetAllCategoriesFromCatalog(string catalogInput, CancellationToken token)
    {

        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);

        var uri = $"https://localhost:44381/api/Category/GetAllCategoriesFromCatalog?catalogName={catalogInput}";


        var response = client.GetAsync(uri, token).Result;


        var contentResult = response.Content.ReadFromJsonAsync<List<CategoryDto>>().Result;

        return contentResult;



    }

    public Guid GetOnlyCategoryGuidByName(string nameInput, CancellationToken token)
    {

        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);

        var uri = $"https://localhost:44381/api/Category/GetOnlyGuidByName?searchName={nameInput}";


        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<Guid>().Result;


        return result;



    }

    public CategoryDto GetSingleCategoryByGuid(Guid categoryGuid, CancellationToken token)
    {

        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);

        var uri = $"https://localhost:44381/api/Category/GetCategoryByGuid?searchGuid={categoryGuid}";

        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<CategoryDto>().Result;


        return result;
    }
    #endregion



}
