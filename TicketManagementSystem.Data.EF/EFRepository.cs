using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TicketManagementSystem.Data.Interfaces;

namespace TicketManagementSystem.Data.EF
{
    public abstract class EFRepository<T> : IRepository<T> where T : class
    {
        public EFRepository(AppDbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }

        protected AppDbContext _dbContext;
        protected DbSet<T> _dbSet;

        public int Count => _dbSet.Count();

        public bool IsEmpty() => !_dbSet.Any();

        public bool ExistsById(int id) => _dbSet.Find(id) != null;

        public bool ExisysById<TEntity>(int id) where TEntity : class
        {
            return _dbContext.Set<TEntity>().Find(id) != null;
        }

        #region Is contains methods

        public bool Contains(T item)
        {
            return _dbSet.Contains(item);
        }

        public bool Contains(Func<T, bool> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public bool Contains<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            return _dbContext.Set<TEntity>().Any(predicate);
        }

        #endregion

        #region Read (Get) methods

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T GetLastItem()
        {
            return _dbSet.Last();
        }

        #endregion

        #region CUD operations (withot Read)
        public void Create(T item)
        {
            _dbSet.Add(item);
            SaveChanges();
        }

        public void Update(T item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            SaveChanges();
        }

        public void Remove(T item)
        {
            _dbSet.Remove(item);
            SaveChanges();
        }
        #endregion

        public void SaveChanges()
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
