using UcommerceWildBearDTO;
namespace WildBearAdventuresMVC.WildBear


{
    public class WildBearApiClient
    {
        public ProductDto GetRandomProductFromCategory(string categoryInput, CancellationToken token)
        {

            var client = new HttpClient();

            var uri = $"https://localhost:44381/api/Product/{categoryInput}";

            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<List<ProductDto>>().Result;


            var randomIndex = new Random().Next(result.Count);

            return result[randomIndex];



        }


    }
}
