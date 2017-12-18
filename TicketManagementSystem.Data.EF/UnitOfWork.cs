using System;
using System.Threading.Tasks;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;
using TicketManagementSystem.Data.EF.Repositories;

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

        private IRepository<Summary> _summary;
        private IRepository<Report> _reports;
        private IRepository<TodoTask> _tasks;

        private IRepository<Color> _colours;
        private IRepository<Serial> _series;
        private IRepository<Package> _packages;
        private IRepository<Ticket> _tickets;

        public void ExecuteSql(string request, params object[] parameters)
        {
            _db.Database.ExecuteSqlCommand(request, parameters);
        }

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

        public void SaveChanges(Action action)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.SaveChanges();
                    action.Invoke();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    await _db.SaveChangesAsync();
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

        public IRepository<Summary> Summary
        {
            get => _summary ?? (_summary = new EFRepository<Summary>(_db));
        }

        public IRepository<Report> Reports
        {
            get => _reports ?? (_reports = new EFRepository<Report>(_db));
        }

        public IRepository<TodoTask> Tasks
        {
            get => _tasks ?? (_tasks = new EFRepository<TodoTask>(_db));
        }

        public IRepository<Color> Colours
        {
            get => _colours ?? (_colours = new ColorRepository(_db));
        }

        public IRepository<Serial> Series
        {
            get => _series ?? (_series = new SerialRepository(_db));
        }

        public IRepository<Package> Packages
        {
            get => _packages ?? (_packages = new PackageRepository(_db));
        }

        public IRepository<Ticket> Tickets
        {
            get => _tickets ?? (_tickets = new TicketRepository(_db));
        }

        #endregion

        #region IDisposable

        private bool _disposed = false;

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _db.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
