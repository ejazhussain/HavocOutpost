﻿using System;
using System.Collections.Generic;

namespace HavocApiClients.Models
{
    /**
     * [
     *   {
     *     "id": "00000000-0000-0000-0000-000000000000",
     *     "name": "string",
     *     "score": 0,
     *     "achievementBadge": "string",
     *     "achievementBadgeIcon": "string"
     *   }
     * ]
     */
    public class TriviaSearch
    {
        List<TriviaPlayer> Players
        {
            get;
            set;
        }
    }
}
