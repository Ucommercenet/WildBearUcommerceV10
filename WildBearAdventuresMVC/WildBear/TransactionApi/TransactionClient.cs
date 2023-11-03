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
            if (AuthorizeRequest(token) == false)
            {
                throw new Exception("authorization failed");
            }

            return "TODO_BasketID";
        }


        /// <summary>
        /// Success returns true
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        private bool AuthorizeRequest(CancellationToken token)
        {





            //STEP 1: Create authorizationCode based on StoreAuthenticationModel Ucommerce authentication call
            var client = new HttpClient();
            var storeAuthenticationModel = _storeAuthentication.GetAuthenticationModelForStore();
            var uri = $"{storeAuthenticationModel.BaseUrl}/api/v1/oauth/connect?client_id={storeAuthenticationModel.ClientGuid}&redirect_uri={storeAuthenticationModel.RedirectUrl}&response_type=code";

            var connectResponse = client.GetAsync(uri, token).Result;

            if (connectResponse.StatusCode is not System.Net.HttpStatusCode.Found)
            { return false; }


            //Get the authorizationCode ready for later use in HttpMessage.Content
            var targetUrlUri = new Uri(connectResponse.Headers.Location.OriginalString);
            var authorizationCode = HttpUtility.ParseQueryString(targetUrlUri.Query).Get("code");
            if (authorizationCode is null)
            { return false; }



            //STEP 2: Prepare and Send authorizationCode to Ucommerce
            var authorizeRequest = new HttpRequestMessage(new HttpMethod("POST"), "/api/v1/oauth/token");
            var authorizationToHeadersSuccessful = AddAuthorizationToHeaders(storeAuthenticationModel, authorizeRequest.Headers);
            if (authorizationToHeadersSuccessful is not true)
            { return false; }


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

            var debug1 = authorizeRequest.RequestUri;



            //TODO: Do I need a BaseUrl like on the GET????

            //Sends the authorizeRequest
            var authorizeResponse = client.SendAsync(authorizeRequest, token).Result;


            //STEP 3: Save the returned Authentication token in AuthorizationTokenDetails

            var authorizationDic = authorizeResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            return true;
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
