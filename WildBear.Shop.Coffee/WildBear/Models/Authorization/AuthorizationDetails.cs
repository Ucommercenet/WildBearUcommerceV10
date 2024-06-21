using Newtonsoft.Json;

namespace WildBear.Shop.Coffee.WildBear.Models.Authorization
{
    public class AuthorizationDetails
    {
        [JsonProperty("access_token")]
        public required string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int AccessTokenExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        public DateTime AccessTokenExpiresAt { get; set; }

    }
}