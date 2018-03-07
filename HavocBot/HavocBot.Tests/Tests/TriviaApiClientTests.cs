using HavocApiClients;
using HavocApiClients.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace HavocBot.Tests.Tests
{
    [TestClass]
    public class TriviaApiClientTests
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
        public void TestRegister()
        {
            TriviaRoster triviaRoster = new TriviaRoster()
            {
                TeamId = "19:7f0240ce5cd64e8ea7c04cf6f1ccb693@thread.skype"
            };

            List<TriviaMember> triviaMembers = new List<TriviaMember>()
            {
                new TriviaMember()
                {
                    Id = "846617cd-f4dc-46b4-8106-f24a7a0bccd7",
                    Name = "Tomi Paananen"
                }
            };

            triviaRoster.Members = triviaMembers;

            TriviaRegister triviaRegister =
                _triviaApiClient.RegisterAsync(triviaRoster).Result;

            Assert.AreNotEqual(null, triviaRegister);
            System.Diagnostics.Debug.WriteLine($"Trivia register result: Success == {triviaRegister.Success}, Message == \"{triviaRegister.Message}\"");
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

        [TestMethod]
        public void TestGetLeaderboard()
        {
            TriviaContext triviaContext = new TriviaContext()
            {
                TeamId = "",
                ChannelId = "",
                Locale = "",
                Theme = "",
                EntityId = "",
                SubEntityId = "",
                Upn = "",
                Tid = "",
                GroupId = ""
            };

            TriviaLeaderboard[] triviaLeaderboard =
                _triviaApiClient.GetLeaderboardAsync(triviaContext, true).Result;

            Assert.AreNotEqual(null, triviaLeaderboard);

            if (triviaLeaderboard.Length > 0)
            {
                string teamId = triviaLeaderboard[0].Id;
                Assert.AreEqual(false, string.IsNullOrEmpty(teamId));
                System.Diagnostics.Debug.WriteLine($"Deserialized team ID: {teamId}");

                string teamName = triviaLeaderboard[0].Name;
                Assert.AreEqual(false, string.IsNullOrEmpty(teamName));
                System.Diagnostics.Debug.WriteLine($"Deserialized team name: {teamName}");
            }
        }

        [TestMethod]
        public void TestSearchPlayer()
        {
            TriviaPlayer[] triviaPlayers =
                _triviaApiClient.SearchPlayerAsync("Tomi").Result;

            Assert.AreNotEqual(null, triviaPlayers);

            if (triviaPlayers.Length > 0)
            {
                string playerName = triviaPlayers[0].Name;
                Assert.AreEqual(false, string.IsNullOrEmpty(playerName));
                System.Diagnostics.Debug.WriteLine($"Deserialized player name: {playerName}");
            }
        }
    }
}
