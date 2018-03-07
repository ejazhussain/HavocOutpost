using System;

namespace HavocBot.Models
{
    /**
     * {
     *   "id": "00000000-0000-0000-0000-000000000000",
     *   "name": "string",
     *   "score": 0,
     *   "achievementBadge": "string",
     *   "achievementBadgeIcon": "string"
     * }
     */
    public class TriviaPlayer
    {
        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Score
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
