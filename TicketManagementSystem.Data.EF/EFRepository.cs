using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public virtual bool Contains(Func<T, bool> predicate)
        {
            return _dbSet.Any(predicate);
        }

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

        #region Async methods.
     
        public virtual async Task<int> GetCountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public virtual async Task<bool> IsEmptyAsync()
        {
            return !(await _dbSet.AnyAsync());
        }

        public virtual async Task<bool> ExistsByIdAsync(int id)
        {
            return (await _dbSet.FindAsync(id)) != null;
        }

        public virtual async Task<bool> ContainsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<IQueryable<T>> GetAllAsync()
        {
            return await Task.Run(() => _dbSet);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() => _dbSet.Where(predicate));
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        #endregion
    }
}
