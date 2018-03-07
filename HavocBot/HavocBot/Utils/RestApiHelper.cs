using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HavocBot.Utils
{
    public class RestApiHelper
    {
        public static readonly string ContentTypeJson = "application/json";

        /// <summary>
        /// Executes an HTTP GET operation using the given URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="contentType">The content type (e.g. application/json).</param>
        /// <returns>The response as string or null in case of a failure.</returns>
        public static async Task<string> ExecuteHttpGetAsync(string requestUri, string contentType = null)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                throw new ArgumentNullException("URI cannot be empty");
            }

            System.Diagnostics.Debug.WriteLine($"GET -> {requestUri}");
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
                    httpResponseMessage = await httpClient.GetAsync(requestUri);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        response = await httpResponseMessage.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"HTTP GET failed - status code was: {httpResponseMessage.StatusCode}");
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"HTTP GET failed: {e.Message}");
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
                        System.Diagnostics.Debug.WriteLine($"HTTP POST failed - status code was: {httpResponseMessage.StatusCode}");
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"HTTP POST failed: {e.Message}");
                }
            }

            return response;
        }


        public static async Task<List<TriviaMember>> GetTeamsMembers(Activity activity, string serviceUrl,string havocTeamId)
        {

            var results = new List<TriviaMember>();
            try
            {
                if (!string.IsNullOrEmpty(serviceUrl))
                {
                    var requestUri = $"{serviceUrl}conversations/{havocTeamId}/members";

                    // Fetch the members in the current conversation
                    var connector = new ConnectorClient(new Uri(serviceUrl));

                    var members = await connector.Conversations.GetConversationMembersAsync(activity.Conversation.Id);
                    foreach (var member in members.AsTeamsChannelAccounts())
                    {
                        results.Add(new TriviaMember()
                        {
                            Id = member != null ? member.Id : string.Empty,
                            Name = member != null ? member.Name : string.Empty
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get members: {ex.Message}");
                return null;
            }

            return results;
        }
    }
}