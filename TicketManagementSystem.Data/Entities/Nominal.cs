using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagementSystem.Data.Entities
{
    [Table("Nominal")]
    public class Nominal
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public bool IsDefault { get; set; }

        public IList<Package> Packages { get; set; }
        public IList<Ticket> Tickets { get; set; }
    }
}
