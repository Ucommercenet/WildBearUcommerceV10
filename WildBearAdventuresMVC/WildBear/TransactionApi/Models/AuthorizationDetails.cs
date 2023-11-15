using Newtonsoft.Json;

namespace WildBearAdventuresMVC.WildBear.TransactionApi.Models
{
    public class AuthorizationDetails
    {
        [JsonProperty("access_token")]
        public required string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int AccessTokenExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        //TODO: This is not a JsonProperty?
        [JsonProperty("expires_at")]
        public DateTime AccessTokenExpiresAt { get; set; }

    }
}