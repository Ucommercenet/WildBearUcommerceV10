using WildBearAdventures.MVC.WildBear.Models.DTOs;

namespace WildBearAdventures.MVC.WildBear.TransactionApi
{
    public interface IStoreApiClient
    {
        List<CategoryDto> GetAllCategoriesFromCatalog(string catalogInput, CancellationToken token);
        List<ProductDto> GetAllProductsFromCategoryGuid(Guid categoryGuid, CancellationToken token);
        Guid GetOnlyCategoryGuidByName(string nameInput, CancellationToken token);
        ProductDto GetRandomProductFromCategory(string categoryName, CancellationToken token);
        CategoryDto GetSingleCategoryByGuid(Guid categoryGuid, CancellationToken token);
        ProductDto GetSingleProductByGuid(Guid productGuid, CancellationToken token);
        ProductDto GetSingleProductByName(string searchName, CancellationToken token);


    }
}