using System;
using System.Collections.Generic;

namespace HavocBot.Datastore
{
    public class InMemoryTriviaDatastore
    {
        /// <summary>
        /// Pending trivia question.
        /// The key is the user ID and the value is the question ID.
        /// </summary>
        public Dictionary<string, int> PendingTriviaQuestions
        {
            get;
            private set;
        }

        public InMemoryTriviaDatastore()
        {
            PendingTriviaQuestions = new Dictionary<string, int>();
        }
    }
}