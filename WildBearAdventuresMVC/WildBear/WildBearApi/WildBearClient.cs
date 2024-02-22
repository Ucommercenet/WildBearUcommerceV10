using WildBearAdventures.MVC.WildBear.Models.DTOs;
using WildBearAdventures.MVC.WildBear.TransactionApi;

namespace WildBearAdventures.MVC.WildBear.WildBearApi;



public class WildBearClient : IStoreApiClient
{
    private readonly StoreAuthorization _storeAuthorizationFlow;

    public WildBearClient(StoreAuthorization storeAuthorizationFlow)
    {
        _storeAuthorizationFlow = storeAuthorizationFlow;
    }

    #region Product Related
    public ProductDto GetRandomProductFromCategory(string categoryName, CancellationToken token)
    {
        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);
        
        var categoryGuid = GetOnlyCategoryGuidByName(categoryName, token);
        var uri = $"api/Product/GetAllProductsFromCategoryGuid?categoryId={categoryGuid}";
        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<List<ProductDto>>().Result;
        var randomIndex = new Random().Next(result.Count);

        return result[randomIndex];

    }
    public List<ProductDto> GetAllProductsFromCategoryGuid(Guid categoryGuid, CancellationToken token)
    {

        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);
        var uri = $"api/Product/GetAllProductsFromCategoryGuid?categoryId={categoryGuid}";

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

        var uri = $"api/Product/GetProductByGuid?searchGuid={productGuid}";

        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<ProductDto>().Result;


        return result;
    }

    public ProductDto GetSingleProductByName(string searchName, CancellationToken token)
    {

        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);

        var uri = $"api/Product/GetProductByName?searchName={searchName}";

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

        var uri = $"api/Category/GetAllCategoriesFromCatalog?catalogName={catalogInput}";
        var response = client.GetAsync(uri, token).Result;


        var contentResult = response.Content.ReadFromJsonAsync<List<CategoryDto>>().Result;

        return contentResult;



    }

    public Guid GetOnlyCategoryGuidByName(string nameInput, CancellationToken token)
    {

        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);

        var uri = $"api/Category/GetOnlyGuidByName?searchName={nameInput}";


        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<Guid>().Result;


        return result;



    }

    public CategoryDto GetSingleCategoryByGuid(Guid categoryGuid, CancellationToken token)
    {

        using var client = _storeAuthorizationFlow.GetAuthorizedClient(token);

        var uri = $"api/Category/GetCategoryByGuid?searchGuid={categoryGuid}";

        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<CategoryDto>().Result;


        return result;
    }
    #endregion



}
