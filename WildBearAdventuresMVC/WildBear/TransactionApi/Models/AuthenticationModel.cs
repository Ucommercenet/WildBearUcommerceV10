namespace WildBearAdventuresMVC.WildBear.TransactionApi.Models
{

    /// <summary>
    /// AuthenticationModel is per store.
    /// </summary>
    public class AuthenticationModel
    {
        public string StoreGuid { get; init; }
        public string ClientSecret { get; init; }

        public AuthenticationTokenDetails authenticationToken { get; init; }


    }
}
