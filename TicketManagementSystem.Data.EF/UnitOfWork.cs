using System;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Data.EF
{
   public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        public UnitOfWork(string connectionString)
        {
            _db = new AppDbContext(connectionString);
        }

        private AppDbContext _db;

        private IRepository<User> _users;
        private IRepository<Login> _logins;
        private IRepository<Role> _roles;
        private IRepository<Color> _colours;
        private IRepository<Serial> _series;
        private IRepository<Package> _packages;
        private IRepository<Ticket> _tickets;

        public void SaveChanges()
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        #region Repositories get properties

        public IRepository<User> Users
        {
            get => _users ?? (_users = new EFRepository<User>(_db));
        }

        public IRepository<Login> Logins
        {
            get => _logins ?? (_logins = new EFRepository<Login>(_db));
        }

        public IRepository<Role> Roles
        {
            get => _roles ?? (_roles = new EFRepository<Role>(_db));
        }

        public IRepository<Color> Colours
        {
            get => _colours ?? (_colours = new EFRepository<Color>(_db));
        }

        public IRepository<Serial> Series
        {
            get => _series ?? (_series = new EFRepository<Serial>(_db));
        }

        public IRepository<Package> Packages
        {
            get => _packages ?? (_packages = new EFRepository<Package>(_db));
        }

        public IRepository<Ticket> Tickets
        {
            get => _tickets ?? (_tickets = new EFRepository<Ticket>(_db));
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
