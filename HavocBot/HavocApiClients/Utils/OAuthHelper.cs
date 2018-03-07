using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace HavocApiClients.Utils
{
    public class OAuthHelper
    {
        private const int DefaultTokenRenewalIntervalInMinutes = 9;

        public string AccessToken
        {
            get;
            protected set;
        }

        private Timer _accessTokenRenewTimer;
        private string _accessTokenUri;
        private string _subscriptionKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessTokenUri"></param>
        /// <param name="subscriptionKey"></param>
        /// <param name="tokenRenewalIntervalInMinutes"></param>
        public OAuthHelper(
            string accessTokenUri, string subscriptionKey,
            int tokenRenewalIntervalInMinutes = DefaultTokenRenewalIntervalInMinutes)
        {
            _accessTokenUri = accessTokenUri;
            _subscriptionKey = subscriptionKey;

            AccessToken = RetrieveAccessToken(_accessTokenUri, _subscriptionKey);

            _accessTokenRenewTimer = new Timer(
                new TimerCallback(OnAccessTokenRenewCallback),
                this,
                TimeSpan.FromMinutes(tokenRenewalIntervalInMinutes),
                TimeSpan.FromMinutes(tokenRenewalIntervalInMinutes));
        }

        private void OnAccessTokenRenewCallback(object stateInfo)
        {
            try
            {
                AccessToken = RetrieveAccessToken(_accessTokenUri, _subscriptionKey);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to renew the access token: {e.Message}");
            }
        }

        private string RetrieveAccessToken(string accessTokenUri, string subscriptionKey)
        {
            WebRequest webRequest = WebRequest.Create(accessTokenUri);
            webRequest.ContentType = RestApiHelper.ContentTypeJson;
            webRequest.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            webRequest.Method = "POST";

            byte[] bytes = Encoding.ASCII.GetBytes(
                string.Format("Subscription-Key={0}", WebUtility.UrlEncode(subscriptionKey)));

            webRequest.ContentLength = bytes.Length;

            using (Stream requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            string accessToken = null;

            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (var reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    accessToken = reader.ReadToEnd();
                }
            }

            System.Diagnostics.Debug.WriteLine((string.IsNullOrEmpty(accessToken)
                ? "Failed to retrieve the access token"
                : $"Retrieved access token: {accessToken}"));

            return accessToken;
        }
    }
}