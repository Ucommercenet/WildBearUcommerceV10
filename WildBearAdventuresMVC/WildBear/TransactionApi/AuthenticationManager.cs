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
        /// If no AuthenticationModel for store is found, a new will be created and returned.
        /// </summary>
        /// <param name="TODO_storeId"></param>
        /// <returns></returns>
        public AuthenticationModel DRAFT_GetAuthenticationModelForStore(string TODO_storeId)
        {

            var foo = _configuration.GetValue<string>("TestValue");


            var result = new AuthenticationModel()
            {
                StoreGuid = "c9baba06-d0d7-4f60-812a-f9d16b3f098c",
                authenticationDetails = new AuthenticationDetails()
                { }
            };


            return result;
        }



        //Authentication is per store the store Id is used as clientID  
        private readonly string _clientID = "c9baba06-d0d7-4f60-812a-f9d16b3f098c";

        private readonly string _PrimarySecret = "99F0E112-586E-44B4-A22B-F5C687E76889pkPdRblQH1i389c7wJG4gsLcpTDKouTLPZ6630T8hXDa7uTBvuxUYC1QEQn6cLGfdNCKTEj9Gu7SSe2XKRFsvL9Es6INsCO7OTUCuI8c55uKhtNz58KhFzG0DpjW2C2BkN";




    }
}
