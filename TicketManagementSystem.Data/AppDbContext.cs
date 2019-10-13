using System.Data.Entity;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Report> Reports { get; set; }
        public DbSet<Summary> Summary { get; set; }

        public DbSet<Color> Colors { get; set; }
        public DbSet<Serial> Series { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
