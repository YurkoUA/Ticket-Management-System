using System.Data.Entity;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Data.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Color> Colours { get; set; }
        public DbSet<Serial> Series { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
