using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TicketManagementSystem.Data.Interfaces;

namespace TicketManagementSystem.Data.EF
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        public EFRepository(AppDbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }

        protected AppDbContext _dbContext;
        protected DbSet<T> _dbSet;

        public virtual int Count => _dbSet.Count();

        public virtual bool IsEmpty() => !_dbSet.Any();

        public virtual bool ExistsById(int id) => _dbSet.Find(id) != null;

        public virtual bool ExisysById<TEntity>(int id) where TEntity : class
        {
            return _dbContext.Set<TEntity>().Find(id) != null;
        }

        #region Is contains methods

        public virtual bool Contains(T item)
        {
            return _dbSet.Contains(item);
        }

        public virtual bool Contains(Func<T, bool> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public virtual bool Contains<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
        {
            return _dbContext.Set<TEntity>().Any(predicate);
        }

        #endregion

        #region Read (Get) methods

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public virtual IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual T GetLastItem()
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

        public virtual void SaveChanges()
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
