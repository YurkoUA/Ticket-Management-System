using System;

namespace TicketManagementSystem.Data.EF.Models
{
    public class Login
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public string UserAgent { get; set; }
        public string Host { get; set; }
        public string Type { get; set; }

        public virtual User User { get; set; }
    }
}
