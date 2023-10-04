using Newtonsoft.Json;

namespace WildBearAdventuresMVC.WildBearRelated.DTO
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class SimpleUcommerceProductDTO
    {
        [JsonProperty("id")]
        public string? id;

        [JsonProperty("sku")]
        public string? sku;

        [JsonProperty("name")]
        public string? name;

        [JsonProperty("shortDescription")]
        public string? shortDescription;
    }





}
