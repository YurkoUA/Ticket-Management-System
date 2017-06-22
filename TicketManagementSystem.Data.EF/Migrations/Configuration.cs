namespace TicketManagementSystem.Data.EF.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TicketManagementSystem.Data.EF.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TicketManagementSystem.Data.EF.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TicketManagementSystem.Data.EF.AppDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.Add(new Role { Name = "User", Description = "Користувач" });
                context.Roles.Add(new Role { Name = "Admin", Description = "Адміністратор" });
                context.SaveChanges();
            }
        }
    }
}
