using System.Data.Entity;
using TicketManagementSystem.AutoTest.Data.Entities;

namespace TicketManagementSystem.AutoTest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
    }
}
