using HavocBot.Models;
using HavocBot.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HavocBot.DAL
{
    public class TriviaApiClient
    {
        private static readonly string TriviaBaseUri = "https://msopenhackeu.azurewebsites.net";
        private static readonly string TriviaQuestionUri = TriviaBaseUri + "/api/trivia/question";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triviaPlayer"></param>
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
