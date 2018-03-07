using System;

namespace HavocBot.Models
{
    /**
     * {
     *   "correct": true,
     *   "achievementBadge": "string",
     *   "achievementBadgeIcon": "string"
     * }
     */
    public class TriviaAnswer
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
