using System;
using System.Threading.Tasks;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Data.EF.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Login> Logins { get; }
        IRepository<Role> Roles { get; }

        IRepository<Summary> Summary { get; }
        IRepository<Report> Reports { get; }
        IRepository<TodoTask> Tasks { get; }

        IRepository<Color> Colours { get; }
        IRepository<Serial> Series { get; }
        IRepository<Package> Packages { get; }
        IRepository<Ticket> Tickets { get; }

        void ExecuteSql(string request, params object[] parameters);

        void SaveChanges();
        void SaveChanges(Action action);

        Task SaveChangesAsync();
    }
}
