﻿using WildBearAdventuresMVC.WildBear.Interfaces;
using WildBearAdventuresMVC.WildBear.Models.DTOs;

namespace WildBearAdventuresMVC.WildBear.WildBearApi;



public class WildBearApiClient : IStoreApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WildBearApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    #region Product Related
    public ProductDto GetRandomProductFromCategory(Guid categoryGuid, CancellationToken token)
    {

        using var client = _httpClientFactory.CreateClient();

        var uri = $"https://localhost:44381/api/Product/GetAllProductsFromCategoryGuid?categoryId={categoryGuid}";

        var response = client.GetAsync(uri, token).Result;

        var resultTEMP = response.Content.ReadAsStringAsync().Result;

        var result = response.Content.ReadFromJsonAsync<List<ProductDto>>().Result;


        var randomIndex = new Random().Next(result.Count);

        return result[randomIndex];

    }
    public List<ProductDto> GetAllProductsFromCategoryGuid(Guid categoryGuid, CancellationToken token)
    {

        using var client = _httpClientFactory.CreateClient();
        var uri = $"https://localhost:44381/api/Product/GetAllProductsFromCategoryGuid?categoryId={categoryGuid}";

        var response = client.GetAsync(uri, token).Result;

        //IMPROVE: Fix breaks if the there is zero products in the list
        //var result = response.Content.ReadFromJsonAsync<List<ProductDto?>>().Result;
        var allProducts = response.Content.ReadFromJsonAsync<List<ProductDto?>>().Result;

        var productsVariants = allProducts.Where(x => x.ParentProductId != null).ToList();



        return productsVariants;

    }
    public ProductDto GetSingleProductByGuid(Guid productGuid, CancellationToken token)
    {

        using var client = _httpClientFactory.CreateClient();

        var uri = $"https://localhost:44381/api/Product/GetProductByGuid?searchGuid={productGuid}";

        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<ProductDto>().Result;


        return result;
    }

    public ProductDto GetSingleProductByName(string searchName, CancellationToken token)
    {

        using var client = _httpClientFactory.CreateClient();

        var uri = $"https://localhost:44381/api/Product/GetProductByName?searchName={searchName}";

        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<ProductDto>().Result;


        return result;
    }


    #endregion


    #region Category Related
    public List<CategoryDto> GetAllCategoriesFromCatalog(string catalogInput, CancellationToken token)
    {

        using var client = _httpClientFactory.CreateClient();

        var uri = $"https://localhost:44381/api/Category/GetAllCategoriesFromCatalog?catalogName={catalogInput}";


        var response = client.GetAsync(uri, token).Result;
               

        var contentResult = response.Content.ReadFromJsonAsync<List<CategoryDto>>().Result;
        
        return contentResult;



    }

    //Optimize: Shoudl I just do better select on GetCategoryByName 
    public Guid GetOnlyCategoryGuidByName(string nameInput, CancellationToken token)
    {

        using var client = _httpClientFactory.CreateClient();

        var uri = $"https://localhost:44381/api/Category/GetOnlyGuidByName?searchName={nameInput}";


        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<Guid>().Result;


        return result;



    }

    public CategoryDto GetSingleCategoryByGuid(Guid categoryGuid, CancellationToken token)
    {

        using var client = _httpClientFactory.CreateClient();

        var uri = $"https://localhost:44381/api/Category/GetCategoryByGuid?searchGuid={categoryGuid}";

        var response = client.GetAsync(uri, token).Result;
        var result = response.Content.ReadFromJsonAsync<CategoryDto>().Result;


        return result;
    }
    #endregion


}
