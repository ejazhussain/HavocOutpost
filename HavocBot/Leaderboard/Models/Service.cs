using HavocApiClients;
using HavocApiClients.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Leaderboard.Models
{
    public class Service
    {
        public static readonly string HavocTeamId = "19:7f0240ce5cd64e8ea7c04cf6f1ccb693@thread.skype";


        

        public async Task<List<LeaderboardItem>> PopulateAsync()
        {

            List<LeaderboardItem> results = new List<LeaderboardItem>();
            

            TriviaApiClient triviaApiClient = new TriviaApiClient();
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

            TriviaLeaderboard[] triviaLeaderboards =
                 await triviaApiClient.GetLeaderboardAsync(triviaContext, true);

            //TriviaLeaderboard[] triviaLeaderboard =
            //    _triviaApiClient.GetLeaderboardAsync(triviaContext, true).Result;


            if (triviaLeaderboards != null)
            {
                foreach (TriviaLeaderboard triviaLeaderboard in triviaLeaderboards)
                {
                    results.Add(new LeaderboardItem()
                    {
                        ProfileImageUri = "",
                        Name = triviaLeaderboard.Name
                    });
                }
            }

            return results;
        }
    }
}