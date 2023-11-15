using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Web;
using WildBearAdventuresMVC.WildBear.TransactionApi.Models;

namespace WildBearAdventuresMVC.WildBear.TransactionApi
{
    public class TransactionClient : ITransactionClient
    {

        private readonly IStoreAuthentication _storeAuthentication;
        private readonly IHttpClientFactory _httpClientFactory;


        public TransactionClient(IStoreAuthentication storeAuthentication, IHttpClientFactory httpClientFactory)
        {
            _storeAuthentication = storeAuthentication;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> CreateBasket(string currency, string cultureCode, CancellationToken cancellationToken)
        {
            AuthorizeFlow(cancellationToken);

            if (_storeAuthentication.WildBearStore.authorizationDetails.AccessToken is null)
            { throw new SecurityException(); }

            //TODO: make wrapper method that retruns a client with baseAdress
            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_storeAuthentication.WildBearStore.BaseUrl);


            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(scheme: "Bearer", parameter: _storeAuthentication.WildBearStore.authorizationDetails.AccessToken);

            var payload = new Dictionary<string, string> { { "currency", currency }, { "cultureCode", cultureCode } };
            var createBasketResponse = await client.PostAsJsonAsync("/api/v1/baskets", payload);

            if (createBasketResponse.IsSuccessStatusCode is false)
            { throw new Exception($"Couldn't create new Basket"); }

            var httpContent = createBasketResponse.Content;


            return "TODO_BasketID";
        }


        #region Authorize Related
        private void AuthorizeFlow(CancellationToken cancellationToken)
        {
            if (_storeAuthentication.WildBearStore.authorizationDetails is null)
            {
                RequestAuthorization(cancellationToken);
            }

            var expiresAt = _storeAuthentication.WildBearStore.authorizationDetails?.AccessTokenExpiresAt;
            //A small buffer as been added
            //Note: Comparing datetime with null always produces false, which is what we want here.
            var tokenIsValid = DateTime.UtcNow.AddSeconds(30) < expiresAt;

            if (tokenIsValid is not true)
            {
                RefreshAuthorization(cancellationToken);
            }
        }


        /// <summary>
        /// Success returns true
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        private void RequestAuthorization(CancellationToken cancellationToken)
        {
            //STEP 1: Create authorizationCode based on StoreAuthenticationModel Ucommerce authentication call         

            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_storeAuthentication.WildBearStore.BaseUrl);

            var storeAuthentication = _storeAuthentication.WildBearStore;
            var uri = $"/api/v1/oauth/connect?client_id={storeAuthentication.ClientGuid}&redirect_uri={storeAuthentication.RedirectUrl}&response_type=code";

            var connectResponse = client.GetAsync(uri, cancellationToken).Result;

            if (connectResponse.StatusCode is not System.Net.HttpStatusCode.Found)
            { throw new SecurityException(); }


            //Get the authorizationCode ready for later use in HttpMessage.Content
            var targetUrlUri = new Uri(connectResponse.Headers.Location.OriginalString);
            var authorizationCode = HttpUtility.ParseQueryString(targetUrlUri.Query).Get("code");
            if (authorizationCode is null)
            { throw new SecurityException(); }


            //STEP 2: Prepare and Send authorizationCode to Ucommerce
            var authorizeRequest = new HttpRequestMessage(new HttpMethod("POST"), "/api/v1/oauth/Token");
            var successful = AddAuthorizationToHeaders(storeAuthentication, authorizeRequest.Headers);
            if (successful is not true)
            { throw new SecurityException(); }


            //Prepares the data and assigns it to HttpRequstMessage.Content
            //According to connect flow documentation: https://docs.ucommerce.net/ucommerce/v9.7/headless/getting-started/quick-start.html
            var dictionary = new Dictionary<string, string>
            {
                            { "code", authorizationCode },
                            { "grant_type", "authorization_code" },
                            { "redirect_uri", storeAuthentication.RedirectUrl }
            };

            authorizeRequest.Content = new FormUrlEncodedContent(dictionary);
            authorizeRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            var authorizeResponse = client.SendAsync(authorizeRequest, cancellationToken).Result;


            //STEP 3: Get the authorizationDetails
            var authorizationDetails = authorizeResponse.Content.ReadAsAsync<AuthorizationDetails>().Result;
            //Update ExpiresAt before save.
            authorizationDetails.AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(authorizationDetails.AccessTokenExpiresIn);
            //Saves the token and other details
            storeAuthentication.authorizationDetails = authorizationDetails;

        }

        private void RefreshAuthorization(CancellationToken cancellationToken)
        {
            //TODO: Check if Auth is ready for refresh
            using var client = _httpClientFactory.CreateClient();
            var storeAuthentication = _storeAuthentication.WildBearStore;


            var refreshRequest = new HttpRequestMessage(new HttpMethod("POST"), "/api/v1/oauth/cancellationToken");

            var succesfull = AddAuthorizationToHeaders(storeAuthentication, refreshRequest.Headers);
            if (succesfull is not true)
            { throw new SecurityException(); }

            var refreshToken = _storeAuthentication.WildBearStore.authorizationDetails.RefreshToken;

            //Prepares the data and assigns it to HttpRequstMessage.Content
            //According to connect flow documentation: https://docs.ucommerce.net/ucommerce/v9.7/headless/getting-started/quick-start.html
            var dictionary = new Dictionary<string, string>
            {
                { "refresh_token", refreshToken },
                { "grant_type", "refresh_token" }
            };
            refreshRequest.Content = new FormUrlEncodedContent(dictionary);
            refreshRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var refreshResponse = client.SendAsync(refreshRequest, cancellationToken).Result;
            //Get the authorizationDetails
            var authorizationDetails = refreshResponse.Content.ReadAsAsync<AuthorizationDetails>().Result;
            //Update ExpiresAt before save.
            authorizationDetails.AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(authorizationDetails.AccessTokenExpiresIn);
            //Saves the token and other details
            storeAuthentication.authorizationDetails = authorizationDetails;

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

        #endregion


    }

}
