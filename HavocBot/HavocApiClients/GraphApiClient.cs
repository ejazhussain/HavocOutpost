using HavocApiClients.Utils;
using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HavocApiClients
{
    public class GraphApiClient
    {
        public static readonly string[] Scopes = new string[] { "user.read" };
        private const string MicrosoftLoginEndpoint = "https://login.microsoftonline.com/common";
        private const string GraphApi10MeEndpoint = "https://graph.microsoft.com/v1.0/me";

        public PublicClientApplication PublicClientApplication
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationId">The application ID.</param>
        public GraphApiClient(string applicationId)
        {
            if (string.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentNullException("Application ID cannot be empty");
            }

            PublicClientApplication =
                new PublicClientApplication(applicationId, MicrosoftLoginEndpoint, TokenCacheHelper.GetUserCache());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticationResult> AuthenticateAsync()
        {
            AuthenticationResult authenticationResult = null;

            try
            {
                authenticationResult =
                    await PublicClientApplication.AcquireTokenSilentAsync(Scopes, PublicClientApplication.Users.FirstOrDefault());
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync
                // This indicates you need to call AcquireTokenAsync to acquire a token
                System.Diagnostics.Debug.WriteLine($"Failed to acquire token: MsalUiRequiredException: {ex.Message}");

                try
                {
                    authenticationResult = await PublicClientApplication.AcquireTokenAsync(Scopes);
                }
                catch (MsalException msalex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to acquire token: {msalex.Message}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to acquire token: {ex.Message}");
            }

            return authenticationResult;
        }

        /// <summary>
        /// Performs an HTTP GET request to a URL using an HTTP Authorization header.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="token">The token.</param>
        /// <returns>String containing the results of the GET operation.</returns>
        public async Task<string> GetHttpContentWithTokenAsync(string url, string token)
        {
            string content = null;
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = null;

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                // Add the token in Authorization header
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get content: {ex.Message}");
            }

            return content;
        }

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        /// <returns>True, if successful. False otherwise.</returns>
        public bool SignOut()
        {
            if (PublicClientApplication.Users.Any())
            {
                try
                {
                    PublicClientApplication.Remove(PublicClientApplication.Users.FirstOrDefault());
                    return true;
                }
                catch (MsalException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to sign out: {ex.Message}");
                }
            }

            return false;
        }
    }
}
