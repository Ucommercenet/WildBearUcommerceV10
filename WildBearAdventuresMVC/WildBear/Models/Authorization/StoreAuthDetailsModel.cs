namespace WildBearAdventures.MVC.WildBear.Models.Authorization
{

    /// <summary>
    /// Authentication is per store.
    /// </summary>
    public class StoreAuthDetailsModel
    {
        /// <summary>
        /// Will function as clientId for the api calls
        /// </summary>
        public required string ClientGuid { get; init; }

        /// <summary>
        /// Find the ClientSecret in the Ucommerce back office
        /// </summary>
        public required string ClientSecret { get; init; }

        public required string RedirectUrl { get; init; }

        public required string BaseUrl { get; init; }

        public AuthorizationDetails? authorizationDetails { get; set; }


    }
}
