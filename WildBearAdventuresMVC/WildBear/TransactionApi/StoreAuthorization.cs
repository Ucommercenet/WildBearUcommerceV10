using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Web;
using WildBearAdventures.MVC.WildBear.Models.Authorization;

namespace WildBearAdventures.MVC.WildBear.TransactionApi
{
    public class StoreAuthorization
    {
        private readonly StoreAuthDetails _storeAuthDetails;
        private readonly IHttpClientFactory _httpClientFactory;

        public StoreAuthorization(StoreAuthDetails storeAuthDetails, IHttpClientFactory httpClientFactory)
        {
            _storeAuthDetails = storeAuthDetails;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Returns client with accessToken and configured BaseAddress 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="SecurityException"></exception>
        /// <remarks>In a multi store setup this would need an update. Because not its only works for WildBearStore</remarks>
        public HttpClient GetAuthorizedClient(CancellationToken cancellationToken)
        {
            AuthorizeFlow(cancellationToken);

            if (_storeAuthDetails.WildBearStore.authorizationDetails.AccessToken is null)
            { throw new SecurityException(); }
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_storeAuthDetails.WildBearStore.BaseUrl);

            var accessToken = _storeAuthDetails.WildBearStore.authorizationDetails.AccessToken;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: accessToken);
            return client;
        }

        #region Authorize Related
        private void AuthorizeFlow(CancellationToken cancellationToken)
        {
            if (_storeAuthDetails.WildBearStore.authorizationDetails is null)
            {
                RequestAuthorization(cancellationToken);
            }

            var expiresAt = _storeAuthDetails.WildBearStore.authorizationDetails?.AccessTokenExpiresAt;
            //A small buffer as been added
            //TODO: use 10 sec for testing

            //Note: Comparing datetime with null always produces false, which is what we want here.
            var tokenIsValid = DateTime.UtcNow.AddSeconds(290) < expiresAt;

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
            client.BaseAddress = new Uri(_storeAuthDetails.WildBearStore.BaseUrl);

            var storeAuthentication = _storeAuthDetails.WildBearStore;
            var uri = $"/api/v1/oauth/connect?client_id={storeAuthentication.ClientGuid}&redirect_uri={storeAuthentication.RedirectUrl}&response_type=code";

            var connectResponse = client.GetAsync(uri, cancellationToken).Result;

            if (connectResponse.StatusCode is not System.Net.HttpStatusCode.Found)
            { throw new SecurityException("Check the URL whitelist in Ucommerce back office"); }


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
                  
            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_storeAuthDetails.WildBearStore.BaseUrl);

            var storeAuthentication = _storeAuthDetails.WildBearStore;
            var refreshRequest = new HttpRequestMessage(new HttpMethod("POST"), "/api/v1/oauth/Token");

            var successful = AddAuthorizationToHeaders(storeAuthentication, refreshRequest.Headers);
            if (successful is not true)
            { throw new SecurityException("AddAuthorizationToHeaders failed"); }

            var refreshToken = _storeAuthDetails.WildBearStore.authorizationDetails.RefreshToken;

            //Prepares the data and assigns it to HttpRequstMessage.Content
            //According to connect flow documentation: https://docs.ucommerce.net/ucommerce/v9.7/headless/getting-started/quick-start.html
            var dictionary = new Dictionary<string, string>
            {
                { "refresh_token", refreshToken },
                { "grant_type", "refresh_token" }
            };
            refreshRequest.Content = new FormUrlEncodedContent(dictionary);
            refreshRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            //TODO: Look into refresh issue
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
        /// <param name="authDetails"></param>
        /// <param name="authorizeRequestHeaders"></param>
        /// <returns>Returns true if successful</returns>
        private static bool AddAuthorizationToHeaders(StoreAuthDetailsModel authDetails, HttpRequestHeaders authorizeRequestHeaders)
        {
            var Base64CredentialsValue = GenerateBase64CredentialsForAuthorizationHeader(authDetails.ClientGuid, authDetails.ClientSecret);

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
