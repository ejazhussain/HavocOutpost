using System;

namespace HavocBot.Models
{
    /**
     * {
     *   "success": true,
     *   "message": "string"
     * }
     */
    public class TriviaRegister
    {
        public bool Success
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }
    }
}
