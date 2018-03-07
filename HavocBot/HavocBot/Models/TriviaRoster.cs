using System;
using System.Collections.Generic;

namespace HavocBot.Models
{
    /**
     * {
     *   "teamId": "string",
     *   "members": [
     *     {
     *       "id": "string",
     *       "name": "string"
     *     }
     *   ]
     * }
     */
    public class TriviaRoster
    {
        public string TeamId
        {
            get;
            set;
        }

        public List<TriviaMember> Members
        {
            get;
            set;
        }

        public TriviaRoster()
        {
            Members = new List<TriviaMember>();
        }
    }
}
