using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Data.EF.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Color> Colours { get; }
        IRepository<Serial> Series { get; }
        IRepository<Package> Packages { get; }
        IRepository<Ticket> Tickets { get; }

        void SaveChanges();
    }
}
