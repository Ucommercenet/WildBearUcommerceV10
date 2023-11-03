using WildBearAdventuresMVC.WildBear.TransactionApi.Models;

namespace WildBearAdventuresMVC.WildBear.TransactionApi
{
    //TODO: Make AuthenticationManager is implimented useing a singleton patten

    public class StoreAuthentication : IStoreAuthentication

    {
        private readonly IConfiguration _configuration;

        public StoreAuthentication(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        /// <summary>
        /// Authentication is per store the store Id is used as clientID
        /// </summary>
        /// <param name="TODO_storeId"></param>
        /// <remark>There might be more then one store per solution, this method if more stores needs authentication</remark>
        /// <returns></returns>
        public StoreAuthenticationModel GetAuthenticationModelForStore()
        {
            var storeGuid = _configuration.GetValue<string>("Authentication:WildBearStore:Guid");
            var clientSecret = _configuration.GetValue<string>("Authentication:WildBearStore:Secret");
            var redirectUrl = _configuration.GetValue<string>("Authentication:WildBearStore:RedirectUrl");
            var BaseUrl = _configuration.GetValue<string>("Authentication:WildBearStore:BaseUrl");


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
