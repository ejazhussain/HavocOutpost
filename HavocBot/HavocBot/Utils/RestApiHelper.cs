using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HavocBot.Utils
{
    public class RestApiHelper
    {
        /// <summary>
        /// Executes an HTTP GET operation using the given URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>The response as string or null in case of a failure.</returns>
        public static async Task<string> ExecuteHttpGetAsync(string requestUri)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                throw new ArgumentNullException("URI cannot be empty");
            }

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

        /// <summary>
        /// Executes an HTTP POST operation using the given URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpContent">The HTTP content (e.g. parameters).</param>
        /// <param name="contentType">The content type (e.g. application/json).</param>
        /// <returns>The response as string or null in case of a failure.</returns>
        public static async Task<string> ExecuteHttpPostAsync(
            string requestUri, HttpContent httpContent, string contentType = null)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                throw new ArgumentNullException("URI cannot be empty");
            }

            System.Diagnostics.Debug.WriteLine($"POST -> {requestUri}");
            string response = null;

            using (HttpClient httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(contentType))
                {
                    httpClient.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue(contentType));
                }

                HttpResponseMessage httpResponseMessage = null;

                try
                {
                    httpResponseMessage = await httpClient.PostAsync(requestUri, httpContent);

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