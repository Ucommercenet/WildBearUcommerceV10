using UcommerceWildBearDTO;
namespace WildBearAdventuresMVC.WildBear


{
    public class WildBearApiClient
    {

        //Product Related
        public ProductDto GetRandomProductFromCategory(Guid categoryGuid, CancellationToken token)
        {

            var client = new HttpClient();

            var uri = $"https://localhost:44381/api/Product/{categoryGuid}";

            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<List<ProductDto>>().Result;


            var randomIndex = new Random().Next(result.Count);

            return result[randomIndex];

        }
        public List<ProductDto> GetAllProductsFromCategoryByGuid(Guid categoryGuid, CancellationToken token)
        {

            var client = new HttpClient();

            var uri = $"https://localhost:44381/api/Product/{categoryGuid}";

            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<List<ProductDto>>().Result;

            return result;

        }
        public ProductDto GetSingleProductByGuid(Guid categoryGuid, CancellationToken token)
        {

            var client = new HttpClient();

            var uri = $"TODO";

            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<ProductDto>().Result;


            return result;
        }





        //Category Related
        public List<CategoryDto> GetAllCategoriesFromCatalog(string catalogInput, CancellationToken token)
        {

            var client = new HttpClient();

            var uri = $"https://localhost:44381/api/Category/GetAllCategoriesFromCatalog?catalogName={catalogInput}";


            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<List<CategoryDto>>().Result;


            return result;



        }


    }
}
