using HavocBot.Models;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HavocBot.DAL
{
    public class TeamsApiClient
    {
        public static async Task<List<TriviaMember>> GetTeamsMembers(Activity activity, string serviceUrl, string teamId)
        {
            var results = new List<TriviaMember>();

            try
            {
                if (!string.IsNullOrEmpty(serviceUrl))
                {
                    var requestUri = $"{serviceUrl}conversations/{teamId}/members";

                    // Fetch the members in the current conversation
                    var connector = new ConnectorClient(new Uri(serviceUrl));
                    var members = await connector.Conversations.GetConversationMembersAsync(activity.Conversation.Id);

                    foreach (var member in members.AsTeamsChannelAccounts())
                    {
                        results.Add(new TriviaMember()
                        {
                            Id = member != null ? member.Id : string.Empty,
                            Name = member != null ? member.Name : string.Empty
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get members: {ex.Message}");
                return null;
            }

            return results;
        }
    }
}
