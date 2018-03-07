using System;
using System.Collections.Generic;

namespace HavocBot.Models
{
    /**
     * {
     *   "id": 0,
     *   "text": "string"
     * }
     */
    public class TriviaQuestionOptions
    {
        public int Id
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
    }

    /**
     * {
     *   "id": 0,
     *   "text": "string",
     *   "questionOptions": [
     *     {
     *       "id": 0,
     *       "text": "string"
     *     }
     *   ]
     * }
     */
    public class TriviaQuestion
    {
        public int Id
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public List<TriviaQuestionOptions> QuestionOptions
        {
            get;
            set;
        }
    }
}
