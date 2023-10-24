using UcommerceWildBearDTO;
namespace WildBearAdventuresMVC.WildBear


{
    public class WildBearApiClient : IWildBearApiClient
    {
        #region Product Related
        public ProductDto GetRandomProductFromCategory(Guid categoryGuid, CancellationToken token)
        {

            var client = new HttpClient();

            var uri = $"https://localhost:44381/api/Product/GetAllProductsFromCategoryGuid?categoryId={categoryGuid}";

            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<List<ProductDto>>().Result;


            var randomIndex = new Random().Next(result.Count);

            return result[randomIndex];

        }
        public List<ProductDto> GetAllProductsFromCategoryGuid(Guid categoryGuid, CancellationToken token)
        {

            var client = new HttpClient();
            var uri = $"https://localhost:44381/api/Product/GetAllProductsFromCategoryGuid?categoryId={categoryGuid}";

            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<List<ProductDto>>().Result;

            return result;

        }
        public ProductDto GetSingleProductByGuid(Guid guid, CancellationToken token)
        {

            var client = new HttpClient();

            var uri = $"TODO";

            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<ProductDto>().Result;


            return result;
        }
        #endregion


        #region Category Related
        public List<CategoryDto> GetAllCategoriesFromCatalog(string catalogInput, CancellationToken token)
        {

            var client = new HttpClient();

            var uri = $"https://localhost:44381/api/Category/GetAllCategoriesFromCatalog?catalogName={catalogInput}";


            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<List<CategoryDto>>().Result;


            return result;



        }

        public Guid GetOnlyGuidByName(string nameInput, CancellationToken token)
        {

            var client = new HttpClient();

            var uri = $"https://localhost:44381/api/Category/GetOnlyGuidByName?searchName={nameInput}";


            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<Guid>().Result;


            return result;



        }

        public CategoryDto GetSingleCategoryByGuid(Guid categoryGuid, CancellationToken token)
        {

            var client = new HttpClient();

            var uri = $"https://localhost:44381/api/Category/GetCategoryByGuid?searchGuid={categoryGuid}";

            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<CategoryDto>().Result;


            return result;
        }
        #endregion


    }
}
