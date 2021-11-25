using System;

namespace PakaUsers.IdentityAuth
{
    public class Response
    {
        public DateTime Timestamp { get; } = DateTime.Now;
        public string Message { get; set; }
    }
}