using UcommerceWildBearDTO;

namespace WildBearAdventuresMVC.WildBear
{
    public interface IWildBearApiClient
    {
        List<CategoryDto> GetAllCategoriesFromCatalog(string catalogInput, CancellationToken token);
        List<ProductDto> GetAllProductsFromCategoryGuid(Guid categoryGuid, CancellationToken token);
        Guid GetOnlyGuidByName(string nameInput, CancellationToken token);
        ProductDto GetRandomProductFromCategory(Guid categoryGuid, CancellationToken token);
        ProductDto GetSingleProductByGuid(Guid categoryGuid, CancellationToken token);
    }
}