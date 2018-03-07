using System;
using System.Collections.Generic;

namespace HavocApiClients.Models
{
    /**
     * {
     *   "id": 0,
     *   "text": "string"
     * }
     */
    public class TriviaQuestionOption
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

        public List<TriviaQuestionOption> QuestionOptions
        {
            get;
            set;
        }
    }
}
