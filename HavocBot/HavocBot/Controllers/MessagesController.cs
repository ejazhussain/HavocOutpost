using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using HavocBot.Utils;
using System.Text;
using Microsoft.Bot.Connector.Teams;
using HavocBot.Models;
using HavocBot.DAL;

namespace HavocBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        private static string havocTeamId = "19:7f0240ce5cd64e8ea7c04cf6f1ccb693@thread.skype";

        

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            //we need to get serviceURL
            //get list of roaster information for Havoc Team
            //GET / v3 / conversations / 19:ja0cu120i1jod12j @skype.net / members


            string serviceUrl = activity.ServiceUrl;
            var memebers = await RestApiHelper.GetTeamsMembers(activity, serviceUrl, havocTeamId);

           
            if (memebers?.Count > 0)
            {
                var triviaRoster = new TriviaRoster
                {
                    TeamId = havocTeamId,
                    Members = memebers
                };
                var triviaApiClient = new TriviaApiClient();
                var registerMemberResponse = await triviaApiClient.RegisterAsync(triviaRoster);

            }


            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                await HandleSystemMessageAsync(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
        

        private async Task<Activity> HandleSystemMessageAsync(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
               
                if (message.MembersAdded?.Count > 0 && message.MembersAdded[0].Name == null)
                {
                    Activity activity = message.CreateReply("Welcome to the trivia team!");
                    ConnectorClient connectorClient = new ConnectorClient(new Uri(message.ServiceUrl));
                    await connectorClient.Conversations.SendToConversationAsync(activity);
                }

            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}