using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Data.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            //AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory() + "\\TicketManagementSystem.Web\\App_Data");
        }

        public AppDbContext(string connectionString) : base(connectionString)
        {
            //AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory() + "\\TicketManagementSystem.Web\\App_Data");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Serial> Series { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
