﻿using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HavocBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
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
               
                if (message.MembersAdded?.Count > 0)
                {
                    string name = message.MembersAdded[0].Name;
                    Activity activity = null;

                    if (string.IsNullOrEmpty(name))
                    {
                        activity = message.CreateReply("Hi! I am Havoc bot. Welcome to my trivia team!");
                    }
                    else if (!name.Equals(message.Recipient.Name)) // Make sure we are not greeting the bot
                    {
                        activity = message.CreateReply($"Hi {message.MembersAdded[0].Name}! Welcome!");
                    }

                    if (activity != null)
                    {
                        ConnectorClient connectorClient = new ConnectorClient(new Uri(message.ServiceUrl));
                        await connectorClient.Conversations.SendToConversationAsync(activity);
                    }
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