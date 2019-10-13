using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagementSystem.Data.Entities
{
    [Table("Summary")]
    public class Summary
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Tickets { get; set; }
        public int HappyTickets { get; set; }
        public int Packages { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Summary summary)
            {
                return summary.Id == Id;
            }
            return false;
        }
    }
}
