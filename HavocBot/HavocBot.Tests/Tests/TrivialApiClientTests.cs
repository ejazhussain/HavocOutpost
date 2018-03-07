using HavocBot.DAL;
using HavocBot.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavocBot.Tests.Tests
{
    [TestClass]
    public class TrivialApiClientTests
    {
        private TriviaApiClient _triviaApiClient;

        [TestInitialize]
        public void Initialize()
        {
            System.Diagnostics.Debug.WriteLine("Initialize");
            _triviaApiClient = new TriviaApiClient();
        }

        [TestCleanup]
        public void Cleanup()
        {
            System.Diagnostics.Debug.WriteLine("Cleanup");
        }

        [TestMethod]
        public void TestGetQuestion()
        {
            TriviaQuestion triviaQuestion =
                _triviaApiClient.GetQuestionAsync(new TriviaPlayer() { Id = "846617cd-f4dc-46b4-8106-f24a7a0bccd7" }).Result;

            Assert.AreNotEqual(null, triviaQuestion);
            Assert.AreEqual(false, string.IsNullOrEmpty(triviaQuestion.Text));
            System.Diagnostics.Debug.WriteLine($"Deserialized question text: {triviaQuestion.Text}");
            Assert.AreNotEqual(null, triviaQuestion.QuestionOptions);
            Assert.AreNotEqual(0, triviaQuestion.QuestionOptions.Count);
        }
    }
}
