using System;

namespace HavocBot.Models
{
    /**
     * {
     *   "teamId": "string",
     *   "channelId": "string",
     *   "locale": "string",
     *   "theme": "string",
     *   "entityId": "string",
     *   "subEntityId": "string",
     *   "upn": "string",
     *   "tid": "00000000-0000-0000-0000-000000000000",
     *   "groupId": "00000000-0000-0000-0000-000000000000"
     * }
     */
    public class TriviaContext
    {
        public string TeamId
        {
            get;
            set;
        }

        public string ChannelId
        {
            get;
            set;
        }

        public string Locale
        {
            get;
            set;
        }

        public string Theme
        {
            get;
            set;
        }

        public string EntityId
        {
            get;
            set;
        }

        public string SubEntityId
        {
            get;
            set;
        }

        public string Upn
        {
            get;
            set;
        }

        public string Tid
        {
            get;
            set;
        }

        public string GroupId
        {
            get;
            set;
        }
    }
}
