using HavocApiClients;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavocBot.Tests.Tests
{
    [TestClass]
    public class GraphApiClientTests
    {
        private const string ApplicationId = "9987abb6-feeb-4d69-b540-c1ab0f770072";
        private const string GraphEndpointUrl = "https://graph.microsoft.com/v1.0/me";
        private GraphApiClient _graphApiClient;

        [TestInitialize]
        public void Initialize()
        {
            System.Diagnostics.Debug.WriteLine("Initialize");
            _graphApiClient = new GraphApiClient(ApplicationId);
        }

        [TestCleanup]
        public void Cleanup()
        {
            System.Diagnostics.Debug.WriteLine("Cleanup");
        }

        [TestMethod]
        public void TestAuthenticate()
        {
            AuthenticationResult authenticationResult = _graphApiClient.AuthenticateAsync().Result;

            Assert.AreNotEqual(null, authenticationResult);
            Assert.AreEqual(false, string.IsNullOrEmpty(authenticationResult.AccessToken));
            System.Diagnostics.Debug.WriteLine($"Received authentication token: {authenticationResult.AccessToken}");
        }

        [TestMethod]
        public void TestGetData()
        {
            AuthenticationResult authenticationResult = _graphApiClient.AuthenticateAsync().Result;
            Assert.AreNotEqual(null, authenticationResult);
            Assert.AreEqual(false, string.IsNullOrEmpty(authenticationResult.AccessToken));

            string content = _graphApiClient.GetHttpContentWithTokenAsync(GraphEndpointUrl, authenticationResult.AccessToken).Result;
            Assert.AreEqual(false, string.IsNullOrEmpty(content));
            System.Diagnostics.Debug.WriteLine($"Got content: {content}");
        }
    }
}
