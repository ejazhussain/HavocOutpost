using HavocBot.Models;
using HavocBot.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HavocBot.DAL
{
    public class TriviaApiClient
    {
        private static readonly string TriviaBaseUri = "https://msopenhackeu.azurewebsites.net";
        private static readonly string TriviaRegisterUri = TriviaBaseUri + "/api/trivia/register";
        private static readonly string TriviaQuestionUri = TriviaBaseUri + "/api/trivia/question";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="triviaRoster"></param>
        /// <returns></returns>
        public async Task<TriviaRegister> RegisterAsync(TriviaRoster triviaRoster)
        {
            TriviaRegister triviaRegister = null;

            HttpContent httpContent =
                new StringContent(
                    JsonConvert.SerializeObject(triviaRoster),
                    Encoding.UTF8,
                    RestApiHelper.ContentTypeJson);

            string response = null;

            try
            {
                response = await RestApiHelper.ExecuteHttpPostAsync(
                    TriviaQuestionUri, httpContent, RestApiHelper.ContentTypeJson);
                System.Diagnostics.Debug.WriteLine($"Received response: {response}");
                triviaRegister = JsonConvert.DeserializeObject<TriviaRegister>(response);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to register: {e.Message}");
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
    }
}
