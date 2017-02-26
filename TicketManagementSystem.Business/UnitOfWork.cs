using System;
using TicketManagementSystem.Data.EF;
using TicketManagementSystem.Data.Interfaces;
using TicketManagementSystem.Data.Models;

namespace TicketManagementSystem.Business
{
    public sealed class UnitOfWork : IDisposable
    {
        private UnitOfWork(AppDbContext context)
        {
            _db = context;
        }

        private AppDbContext _db;

        private IRepository<User> _users;
        private IRepository<Color> _colours;
        private IRepository<Serial> _series;
        private IRepository<Package> _packages;
        private IRepository<Ticket> _tickets;

        private static UnitOfWork _uowSingleton;

        public static UnitOfWork GetInstance()
        {
            if (_uowSingleton == null)
                _uowSingleton = new UnitOfWork(new AppDbContext());

            return _uowSingleton;
        }

        #region Repositories get properties

        public IRepository<User> Users
        {
            get
            {
                if (_users == null)
                    _users = new EFRepository<User>(_db);

                return _users;
            }
        }

        public IRepository<Color> Colours
        { 
            get
            {
                if (_colours == null)
                    _colours = new EFRepository<Color>(_db);

                return _colours;
            }
        }

        public IRepository<Serial> Series
        {
            get
            {
                if (_series == null)
                    _series = new EFRepository<Serial>(_db);

                return _series;
            }
        }

        public IRepository<Package> Packages
        {
            get
            {
                if (_packages == null)
                    _packages = new EFRepository<Package>(_db);

                return _packages;
            }
        }

        public IRepository<Ticket> Tickets
        {
            get
            {
                if (_tickets == null)
                    _tickets = new EFRepository<Ticket>(_db);

                return _tickets;
            }
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
