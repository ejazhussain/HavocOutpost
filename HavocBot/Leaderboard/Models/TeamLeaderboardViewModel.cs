using HavocApiClients;
using HavocApiClients.Models;
using System;
using System.Collections.Generic;

namespace Leaderboard.Models
{
    public class TeamLeaderboardViewModel
    {
        private string _teamId;
        public string TeamId
        {
            get
            {
                return _teamId;
            }
            set
            {
                if (_teamId == null || !_teamId.Equals(value))
                {
                    _teamId = value;
                    PopulateAsync(_teamId);
                }
            }
        }

        public List<LeaderboardItem> LeaderboardItems
        {
            get;
            private set;
        }

        public TeamLeaderboardViewModel()
        {
            LeaderboardItems = new List<LeaderboardItem>();

            // Add new test item - remove when working
            LeaderboardItems.Add(new LeaderboardItem()
            {
                ProfileImageUri = "https://video-images.vice.com/articles/59bf9f3aacd85953f7b69191/lede/1505732141447-rnm.jpeg?crop=1xw%3A0.8924xh%3B0xw%2C0.0905xh&resize=1250%3A*",
                Name = "Rick"
            });
        }

        private async void PopulateAsync(string teamId)
        {
            LeaderboardItems.Clear();

            TriviaApiClient triviaApiClient = new TriviaApiClient();
            TriviaContext triviaContext = new TriviaContext()
            {
                TeamId = teamId
            };

            TriviaLeaderboard[] triviaLeaderboards =
                await triviaApiClient.GetLeaderboardAsync(triviaContext, /* isTeam */ true);

            if (triviaLeaderboards != null)
            {
                foreach (TriviaLeaderboard triviaLeaderboard in triviaLeaderboards)
                {
                    LeaderboardItems.Add(new LeaderboardItem()
                    {
                        ProfileImageUri = "",
                        Name = triviaLeaderboard.Name
                    });
                }
            }
        }
    }
}