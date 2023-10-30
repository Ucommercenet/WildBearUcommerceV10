using WildBearAdventuresMVC.WildBear.TransactionApi.Models;

namespace WildBearAdventuresMVC.WildBear.TransactionApi
{
    //TODO: Make AuthenticationManager is implimented useing a singleton patten

    public class AuthenticationManager
    {
        private readonly IConfiguration _configuration;

        public AuthenticationManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }




        //TODO: Dynamic get Redirect Url via AuthenticationRedirectUrlEntity for Redirect Url
        //TODO: Dynamic get _PrimarySecret via uCommerce_ProductCatalogGroup

        private List<AuthenticationModel>? _authenticationModels;


        //DRAFT notes: make it work for one store, then more.



        /// <summary>
        /// Authentication is per store the store Id is used as clientID
        /// </summary>
        /// <param name="TODO_storeId"></param>
        /// <remark>There might be more then one store per solution, this method if more stores needs authentication</remark>
        /// <returns></returns>
        public AuthenticationModel GetAuthenticationModelForStore()
        {
            var storeGuid = _configuration.GetValue<string>("Authentication:WildBearStore:Guid");
            var clientSecret = _configuration.GetValue<string>("Authentication:WildBearStore:Secret");

            if (string.IsNullOrWhiteSpace(storeGuid) || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new Exception("Missing authentication information");
            }



            var authenticationModel = new AuthenticationModel()
            {
                StoreGuid = storeGuid,
                ClientSecret = clientSecret,
                authenticationToken = new AuthenticationTokenDetails() { }
            };


            return authenticationModel;
        }

    }
}
