using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HavocBot.Utils
{
    public class RestApiHelper
    {
        /// <summary>
        /// Executes an HTTP GET operation using the given URI.
        /// </summary>
        /// <param name="uri">The request URI.</param>
        /// <returns>The response as string or null in case of a failure.</returns>
        public static async Task<string> ExecuteHttpGet(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("URI cannot be empty");
            }

            string requestUri = Uri.EscapeDataString(uri);
            string response = null;

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponseMessage = null;

                try
                {
                    httpResponseMessage = await httpClient.GetAsync(requestUri);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        response = await httpResponseMessage.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to get response - status code was: {httpResponseMessage.StatusCode}");
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to get response: {e.Message}");
                }
            }

            return response;
        }
    }
}