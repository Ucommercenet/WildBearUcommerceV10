using System.Net.Http.Headers;
using System.Text;
using System.Web;
using WildBearAdventuresMVC.WildBear.TransactionApi.Models;

namespace WildBearAdventuresMVC.WildBear.TransactionApi
{
    public class TransactionClient : ITransactionClient
    {

        private readonly TransactionApi.IStoreAuthentication _storeAuthentication;

        public TransactionClient(IStoreAuthentication storeAuthentication)
        {
            _storeAuthentication = storeAuthentication;
        }

        public async Task<string> CreateBasket(string currency, string cultureCode, CancellationToken token)
        {



            return "TODO_BasketID";
        }

        //Optimize: throw Exception not just return null
        /// <summary>
        /// Success returns true
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        private AuthorizeResponseModel? AuthorizeRequest(CancellationToken token)
        {
            //STEP 1: Create authorizationCode based on StoreAuthenticationModel Ucommerce authentication call         
            var storeAuthenticationModel = _storeAuthentication.GetAuthenticationModelForStore();
            var client = new HttpClient
            {
                BaseAddress = new Uri(storeAuthenticationModel.BaseUrl)
            };

            var uri = $"/api/v1/oauth/connect?client_id={storeAuthenticationModel.ClientGuid}&redirect_uri={storeAuthenticationModel.RedirectUrl}&response_type=code";
            var connectResponse = client.GetAsync(uri, token).Result;

            if (connectResponse.StatusCode is not System.Net.HttpStatusCode.Found)
            { return null; }


            //Get the authorizationCode ready for later use in HttpMessage.Content
            var targetUrlUri = new Uri(connectResponse.Headers.Location.OriginalString);
            var authorizationCode = HttpUtility.ParseQueryString(targetUrlUri.Query).Get("code");
            if (authorizationCode is null)
            { return null; }


            //STEP 2: Prepare and Send authorizationCode to Ucommerce
            var authorizeRequest = new HttpRequestMessage(new HttpMethod("POST"), "/api/v1/oauth/token");
            var authorizationToHeadersSuccessful = AddAuthorizationToHeaders(storeAuthenticationModel, authorizeRequest.Headers);
            if (authorizationToHeadersSuccessful is not true)
            { return null; }


            //Prepares the data and assigns it to HttpRequstMessage.Content
            //According to connect flow documentation: https://docs.ucommerce.net/ucommerce/v9.7/headless/getting-started/quick-start.html
            var dictionary = new Dictionary<string, string>
                        {
                            { "code", authorizationCode },
                            { "grant_type", "authorization_code" },
                            { "redirect_uri", storeAuthenticationModel.RedirectUrl }
            };

            authorizeRequest.Content = new FormUrlEncodedContent(dictionary);
            authorizeRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            var authorizeResponse = client.SendAsync(authorizeRequest, token).Result;


            //STEP 3: Save the returned Authentication token in AuthorizationTokenDetails

            var authorizationModel = authorizeResponse.Content.ReadAsAsync<AuthorizeResponseModel>().Result;
            //Update ExpiresAt
            authorizationModel.ExpiresAt = DateTime.UtcNow.AddSeconds(authorizationModel.ExpiresIn);


            return authorizationModel;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="authentication"></param>
        /// <param name="authorizeRequestHeaders"></param>
        /// <returns>Returns true if successful</returns>
        private static bool AddAuthorizationToHeaders(StoreAuthenticationModel authentication, HttpRequestHeaders authorizeRequestHeaders)
        {
            var Base64CredentialsValue = GenerateBase64CredentialsForAuthorizationHeaderValue(authentication.ClientGuid, authentication.ClientSecret);

            var isAuthAdded = authorizeRequestHeaders.TryAddWithoutValidation(name: "Authorization", value: Base64CredentialsValue);

            return isAuthAdded;
        }

        private static string GenerateBase64CredentialsForAuthorizationHeaderValue(string clientId, string clientSecret)
        {
            var credentials = $"{clientId}:{clientSecret}";
            var credentialsByteData = Encoding.GetEncoding("iso-8859-1").GetBytes(credentials);
            var base64Credentials = Convert.ToBase64String(credentialsByteData);

            return $"Basic {base64Credentials}";
        }



    }

}
