using WildBearAdventures.MVC.WildBear.Models.Authorization;

namespace WildBearAdventures.MVC.WildBear.TransactionApi
{

    /// <summary>
    /// *IMPORTANT*
    /// StoreAuthDetails is a singleton
    /// In this demo, we only have theWildBearStore
    /// Authentication is per store, the store Id is used as clientID
    /// </summary>
    public class StoreAuthDetails
    {

        public StoreAuthDetails(IConfiguration configuration)
        {
            WildBearStore = GetStoreAuthDetailsModelForWildBearStore(configuration);
        }

        public StoreAuthDetailsModel WildBearStore { get; }

        //Handout Part 4: Authentication (No code task) 

        /// <summary>
        /// Authentication is per store the store Id is used as clientID
        /// </summary>        
        /// <remark>There might be more then one store per solution, this method if more stores needs authentication</remark>
        /// <returns></returns>
        private StoreAuthDetailsModel GetStoreAuthDetailsModelForWildBearStore(IConfiguration configuration)
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



            var authenticationModel = new StoreAuthDetailsModel()
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
