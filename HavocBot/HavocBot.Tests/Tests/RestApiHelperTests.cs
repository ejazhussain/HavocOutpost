using HavocApiClients.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Text;

namespace HavocBot.Tests
{
    [TestClass]
    public class RestApiHelperTests
    {
        private const string BaseUri = "https://msopenhackeu.azurewebsites.net";
        private const string TriviaQuestionUri = BaseUri + "/api/trivia/question";
        private const string TriviaSearchUri = BaseUri + "/api/trivia/search?k={0}";

        [TestInitialize]
        public void Initialize()
        {
            System.Diagnostics.Debug.WriteLine("Initialize");
        }

        [TestCleanup]
        public void Cleanup()
        {
            System.Diagnostics.Debug.WriteLine("Cleanup");
        }

        [TestMethod]
        public void TestExecuteGetPost()
        {
            string requestUri = string.Format(TriviaSearchUri, "Tomi");
            string response = RestApiHelper.ExecuteHttpGetAsync(requestUri, RestApiHelper.ContentTypeJson).Result;
            System.Diagnostics.Debug.WriteLine($"Received response: {response}");
        }

        [TestMethod]
        public void TestExecuteHttpPost()
        {
            HttpContent httpContent =
                new StringContent(
                    "{ \"id\": \"846617cd-f4dc-46b4-8106-f24a7a0bccd7\" }",
                    Encoding.UTF8,
                    RestApiHelper.ContentTypeJson);

            string response = RestApiHelper.ExecuteHttpPostAsync(
                TriviaQuestionUri, httpContent, RestApiHelper.ContentTypeJson).Result;
            System.Diagnostics.Debug.WriteLine($"Received response: {response}");
        }
    }
}
