using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TicketManagementSystem.Data.EF.Interfaces;

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

        public virtual int GetCount()
        {
            return _dbSet.Count();
        }

        public virtual int GetCount(Func<T, bool> predicate)
        {
            return _dbSet.Count(predicate);
        }

        public virtual bool IsEmpty()
        {
            return !_dbSet.Any();
        }

        public virtual bool ExistsById(int id)
        {
            return _dbSet.Find(id) != null;
        }

        #region Is contains methods

        public virtual bool Contains(T item)
        {
            return _dbSet.AsEnumerable().Contains(item);
        }

        public virtual bool Contains(Func<T, bool> predicate)
        {
            return _dbSet.Any(predicate);
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
        
        #endregion

        #region CUD operations (withot Read)

        public virtual T Create(T item)
        {
            return _dbSet.Add(item);
        }

        public virtual void Update(T item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        public virtual void Remove(T item)
        {
            _dbSet.Remove(item);
        }

        public virtual void Remove(int id, string table)
        {
            _dbContext.Database.ExecuteSqlCommand($"DELETE FROM {table} WHERE Id = {id}");
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        #endregion
    }
}
