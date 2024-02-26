using Newtonsoft.Json;

namespace WildBearAdventures.MVC.WildBear.Models.Authorization
{
    public class AuthorizationDetails
    {
        [JsonProperty("access_token")]
        public required string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int AccessTokenExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        //TODO: This is not a Property out-of-the-box must be set manually, test is this correct?
        [JsonProperty("expires_at")]
        public DateTime AccessTokenExpiresAt { get; set; }

    }
}