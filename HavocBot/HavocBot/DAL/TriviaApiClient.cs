using HavocBot.Models;
using HavocBot.Utils;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HavocBot.DAL
{
    public class TriviaApiClient
    {
        public static readonly string TriviaBaseUri = "https://msopenhackeu.azurewebsites.net";
        public static readonly string TriviaRegisterUri = TriviaBaseUri + "/api/trivia/register";
        public static readonly string TriviaQuestionUri = TriviaBaseUri + "/api/trivia/question";
        public static readonly string TriviaAnswerUri = TriviaBaseUri + "/api/trivia/answer";
        public static readonly string TriviaLeaderboardTeamUri = TriviaBaseUri + "/api/trivia/leaderboard/team";
        public static readonly string TriviaLeaderboardUserUri = TriviaBaseUri + "/api/trivia/leaderboard/user";
        public static readonly string TriviaSearchUri = TriviaBaseUri + "/api/trivia/search?k={0}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triviaRoster"></param>
        /// <returns></returns>
        public async Task<TriviaRegister> RegisterAsync(TriviaRoster triviaRoster)
        {
            TriviaRegister triviaRegister = null;
            string serializedTriviaRoster = JsonConvert.SerializeObject(triviaRoster);

            HttpContent httpContent =
                new StringContent(
                    serializedTriviaRoster,
                    Encoding.UTF8,
                    RestApiHelper.ContentTypeJson);

            string response = null;

            try
            {
                response = await RestApiHelper.ExecuteHttpPostAsync(
                    TriviaRegisterUri, httpContent, RestApiHelper.ContentTypeJson);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to register: {e.Message}");
            }

            if (string.IsNullOrEmpty(response))
            {
                System.Diagnostics.Debug.WriteLine("The response from register POST operation was null");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Received response: {response}");

                try
                {
                    triviaRegister = JsonConvert.DeserializeObject<TriviaRegister>(response);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to deserialize register response: {e.Message}");
                }
            }

            return triviaRegister;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triviaPlayer">The player (only ID field required).</param>
        /// <returns></returns>
        public async Task<TriviaQuestion> GetQuestionAsync(TriviaPlayer triviaPlayer)
        {
            TriviaQuestion triviaQuestion = null;

            HttpContent httpContent =
                new StringContent(
                    "{ \"id\": \"" + triviaPlayer.Id + "\" }",
                    Encoding.UTF8,
                    RestApiHelper.ContentTypeJson);

            string response = null;

            try
            {
                response = await RestApiHelper.ExecuteHttpPostAsync(
                    TriviaQuestionUri, httpContent, RestApiHelper.ContentTypeJson);
                System.Diagnostics.Debug.WriteLine($"Received response: {response}");
                triviaQuestion = JsonConvert.DeserializeObject<TriviaQuestion>(response);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get question: {e.Message}");
            }

            return triviaQuestion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triviaAnswer"></param>
        /// <returns></returns>
        public async Task<TriviaAnswerResponse> PostAnswerAsync(TriviaAnswer triviaAnswer)
        {
            TriviaAnswerResponse triviaAnswerResponse = null;

            HttpContent httpContent =
                new StringContent(
                    JsonConvert.SerializeObject(triviaAnswer),
                    Encoding.UTF8,
                    RestApiHelper.ContentTypeJson);

            string response = null;

            try
            {
                response = await RestApiHelper.ExecuteHttpPostAsync(
                    TriviaAnswerUri, httpContent, RestApiHelper.ContentTypeJson);
                System.Diagnostics.Debug.WriteLine($"Received response: {response}");
                triviaAnswerResponse = JsonConvert.DeserializeObject<TriviaAnswerResponse>(response);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error occured while posting an answer: {e.Message}");
            }

            return triviaAnswerResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triviaContext"></param>
        /// <param name="isTeam">If true, will look for team. If false, will look for user.</param>
        /// <returns></returns>
        public async Task<TriviaLeaderboard[]> GetLeaderboardAsync(TriviaContext triviaContext, bool isTeam)
        {
            TriviaLeaderboard[] triviaLeaderboard = null;

            string requestUri = isTeam ? TriviaLeaderboardTeamUri : TriviaLeaderboardUserUri;

            HttpContent httpContent =
                new StringContent(
                    JsonConvert.SerializeObject(triviaContext),
                    Encoding.UTF8,
                    RestApiHelper.ContentTypeJson);

            string response = null;

            try
            {
                response = await RestApiHelper.ExecuteHttpPostAsync(
                    requestUri, httpContent, RestApiHelper.ContentTypeJson);
                System.Diagnostics.Debug.WriteLine($"Received response: {response}");
                triviaLeaderboard = JsonConvert.DeserializeObject<TriviaLeaderboard[]>(response);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get leaderboard: {e.Message}");
            }

            return triviaLeaderboard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<TriviaPlayer[]> SearchPlayerAsync(string searchTerm)
        {
            TriviaPlayer[] triviaPlayers = null;

            string requestUri = string.Format(TriviaSearchUri, searchTerm);
            string response = null;

            try
            {
                response = await RestApiHelper.ExecuteHttpGetAsync(requestUri, RestApiHelper.ContentTypeJson);
                System.Diagnostics.Debug.WriteLine($"Received response: {response}");
                triviaPlayers = JsonConvert.DeserializeObject<TriviaPlayer[]>(response);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Search failed: {e.Message}");
            }

            return triviaPlayers;
        }
    }
}
