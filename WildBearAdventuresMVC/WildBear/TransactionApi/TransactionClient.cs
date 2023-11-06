using System.Net.Http.Headers;
using System.Text;
using System.Web;
using WildBearAdventuresMVC.WildBear.TransactionApi.Models;

namespace WildBearAdventuresMVC.WildBear.TransactionApi
{
    public class TransactionClient : ITransactionClient
    {

        private readonly IStoreAuthentication _storeAuthentication;

        public TransactionClient(IStoreAuthentication storeAuthentication)
        {
            _storeAuthentication = storeAuthentication;
        }

        public async Task<string> CreateBasket(string currency, string cultureCode, CancellationToken token)
        {
            AuthorizeCheck(token);


            return "TODO_BasketID";
        }


        private void AuthorizeCheck(CancellationToken cancellationToken)
        {


            if (_storeAuthentication.AuthenticationModel.authorizationDetails is null)
            {
                RequestAuthorization(cancellationToken);
            }

            var expiresAt = _storeAuthentication.AuthenticationModel.authorizationDetails?.ExpiresAt;
            //A small buffer as been added
            //Note: Comparing datetime with null always produces false, which is what we want here.
            var tokenIsValid = DateTime.UtcNow.AddSeconds(30) < expiresAt;


            if (tokenIsValid is not true)
            {
                RefreshAuthorization(cancellationToken);
            }

            //Note: If none of the above are true, the authorizationToken is still valid and no action needed

        }

        private AuthorizationDetails RefreshAuthorization(CancellationToken cancellationToken)
        {



            throw new NotImplementedException();
        }






        //Optimize: throw Exception not just return null
        /// <summary>
        /// Success returns true
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        private bool RequestAuthorization(CancellationToken token)
        {
            //STEP 1: Create authorizationCode based on StoreAuthenticationModel Ucommerce authentication call         
            var authentication = _storeAuthentication.AuthenticationModel;
            var client = new HttpClient
            {
                BaseAddress = new Uri(_storeAuthentication.AuthenticationModel.BaseUrl)
            };

            var uri = $"/api/v1/oauth/connect?client_id={authentication.ClientGuid}&redirect_uri={authentication.RedirectUrl}&response_type=code";
            var connectResponse = client.GetAsync(uri, token).Result;

            if (connectResponse.StatusCode is not System.Net.HttpStatusCode.Found)
            { return false; }


            //Get the authorizationCode ready for later use in HttpMessage.Content
            var targetUrlUri = new Uri(connectResponse.Headers.Location.OriginalString);
            var authorizationCode = HttpUtility.ParseQueryString(targetUrlUri.Query).Get("code");
            if (authorizationCode is null)
            { return false; }


            //STEP 2: Prepare and Send authorizationCode to Ucommerce
            var authorizeRequest = new HttpRequestMessage(new HttpMethod("POST"), "/api/v1/oauth/cancellationToken");
            var authorizationToHeadersSuccessful = AddAuthorizationToHeaders(authentication, authorizeRequest.Headers);
            if (authorizationToHeadersSuccessful is not true)
            { return false; }


            //Prepares the data and assigns it to HttpRequstMessage.Content
            //According to connect flow documentation: https://docs.ucommerce.net/ucommerce/v9.7/headless/getting-started/quick-start.html
            var dictionary = new Dictionary<string, string>
            {
                            { "code", authorizationCode },
                            { "grant_type", "authorization_code" },
                            { "redirect_uri", authentication.RedirectUrl }
            };

            authorizeRequest.Content = new FormUrlEncodedContent(dictionary);
            authorizeRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            var authorizeResponse = client.SendAsync(authorizeRequest, token).Result;


            //STEP 3: Save the returned authorizationDetails
            var authorizationDetails = authorizeResponse.Content.ReadAsAsync<AuthorizationDetails>().Result;
            //Update ExpiresAt before save.
            authorizationDetails.ExpiresAt = DateTime.UtcNow.AddSeconds(authorizationDetails.ExpiresIn);
            //Saves the token and other details
            authentication.authorizationDetails = authorizationDetails;


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
            var Base64CredentialsValue = GenerateBase64CredentialsForAuthorizationHeader(authentication.ClientGuid, authentication.ClientSecret);

            var isAuthAdded = authorizeRequestHeaders.TryAddWithoutValidation(name: "Authorization", value: Base64CredentialsValue);

            return isAuthAdded;
        }

        private static string GenerateBase64CredentialsForAuthorizationHeader(string clientId, string clientSecret)
        {
            var credentials = $"{clientId}:{clientSecret}";
            var credentialsByteData = Encoding.GetEncoding("iso-8859-1").GetBytes(credentials);
            var base64Credentials = Convert.ToBase64String(credentialsByteData);

            return $"Basic {base64Credentials}";
        }



    }

}
