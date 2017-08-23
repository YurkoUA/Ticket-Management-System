using System;

namespace TicketManagementSystem.Business.DTO
{
    public class SummaryDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Tickets { get; set; }
        public int HappyTickets { get; set; }
        public int Packages { get; set; }
    }
}
