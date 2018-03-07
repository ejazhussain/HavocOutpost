using System;

namespace HavocApiClients.Models
{
    /**
     * {
     *   "correct": true,
     *   "achievementBadge": "string",
     *   "achievementBadgeIcon": "string"
     * }
     */
    public class TriviaAnswerResponse
    {
        public bool Correct
        {
            get;
            set;
        }

        public string AchievementBadge
        {
            get;
            set;
        }

        public string AchievementBadgeIcon
        {
            get;
            set;
        }
    }
}
