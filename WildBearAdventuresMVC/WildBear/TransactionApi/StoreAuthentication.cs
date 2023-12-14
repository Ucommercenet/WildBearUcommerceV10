using WildBearAdventuresMVC.WildBear.Models.Authorization;

namespace WildBearAdventuresMVC.WildBear.TransactionApi
{

    /// <summary>
    /// *IMPORTANT*
    /// StoreAuthentication is a singleton
    /// In this demo, we only have theWildBearStore
    /// Authentication is per store, the store Id is used as clientID
    /// </summary>
    public class StoreAuthentication
    {

        public StoreAuthentication(IConfiguration configuration)
        {
            WildBearStore = GetAuthenticationModelForWildBearStore(configuration);
        }

        public StoreAuthenticationModel WildBearStore { get; }



        /// <summary>
        /// Authentication is per store the store Id is used as clientID
        /// </summary>        
        /// <remark>There might be more then one store per solution, this method if more stores needs authentication</remark>
        /// <returns></returns>
        private StoreAuthenticationModel GetAuthenticationModelForWildBearStore(IConfiguration configuration)
        {

            var storeGuid = configuration.GetValue<string>("Authentication:WildBearStore:Guid");
            var clientSecret = configuration.GetValue<string>("Authentication:WildBearStore:Secret");
            var redirectUrl = configuration.GetValue<string>("Authentication:WildBearStore:RedirectUrl");
            var BaseUrl = configuration.GetValue<string>("Authentication:WildBearStore:BaseUrl");


            if (string.IsNullOrWhiteSpace(storeGuid) ||
                string.IsNullOrWhiteSpace(clientSecret) ||
                string.IsNullOrWhiteSpace(redirectUrl) ||
                string.IsNullOrWhiteSpace(BaseUrl))
            {
                throw new Exception("Missing authentication information");
            }



            var authenticationModel = new StoreAuthenticationModel()
            {
                ClientGuid = storeGuid,
                ClientSecret = clientSecret,
                RedirectUrl = redirectUrl,
                BaseUrl = BaseUrl

            };

            return authenticationModel;
        }




    }
}
