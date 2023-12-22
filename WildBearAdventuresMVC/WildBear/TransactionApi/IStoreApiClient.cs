using WildBearAdventuresMVC.WildBear.Models.DTOs;

namespace WildBearAdventuresMVC.WildBear.Interfaces
{
    public interface IStoreApiClient
    {
        List<CategoryDto> GetAllCategoriesFromCatalog(string catalogInput, CancellationToken token);
        List<ProductDto> GetAllProductsFromCategoryGuid(Guid categoryGuid, CancellationToken token);
        Guid GetOnlyCategoryGuidByName(string nameInput, CancellationToken token);
        ProductDto GetRandomProductFromCategory(Guid categoryGuid, CancellationToken token);
        CategoryDto GetSingleCategoryByGuid(Guid categoryGuid, CancellationToken token);
        ProductDto GetSingleProductByGuid(Guid productGuid, CancellationToken token);
        ProductDto GetSingleProductByName(string searchName, CancellationToken token);
    }
}