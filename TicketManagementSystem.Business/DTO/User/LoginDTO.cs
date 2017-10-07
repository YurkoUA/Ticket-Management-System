using System;

namespace TicketManagementSystem.Business.DTO
{
    public class LoginDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public string UserAgent { get; set; }
        public string Host { get; set; }
        public string Type { get; set; }

        public string CutUserAgent()
        {
            var length = 10;

            if (UserAgent.Length < length)
            {
                length = UserAgent.Length;
            }

            return UserAgent.Substring(0, length) + "...";
        }
    }
}
