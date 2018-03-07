using System;

namespace HavocBot.Models
{
    /**
     * {
     *   "userId": "00000000-0000-0000-0000-000000000000",
     *   "questionId": 0,
     *   "answerId": 0
     * }
     */
    public class TriviaAnswer
    {
        public string UserId
        {
            get;
            set;
        }

        public string QuestionId
        {
            get;
            set;
        }

        public string AnswerId
        {
            get;
            set;
        }
    }
}
