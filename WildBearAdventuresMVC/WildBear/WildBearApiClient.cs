using UcommerceWildBearDTO;
namespace WildBearAdventuresMVC.WildBear


{
    public class WildBearApiClient
    {
        public ProductDto GetRandomCoffeeProduct(CancellationToken token)
        {

            var client = new HttpClient();

            var uri = @"https://localhost:44381/api/Product/WildCoffee";

            var response = client.GetAsync(uri, token).Result;
            var result = response.Content.ReadFromJsonAsync<List<ProductDto>>().Result;


            var randomIndex = new Random().Next(result.Count);

            return result[randomIndex];



        }


    }
}
