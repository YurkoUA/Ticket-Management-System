using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TicketManagementSystem.Infrastructure.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> FindAllAsync();
        Task<IQueryable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IQueryable<TEntity>> FindAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IQueryable<TEntity>> FindAllIncludingAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IQueryable<TEntity>> FindAllIncludingAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties);

        Task<TEntity> FindByIdAsync(int id);
        Task<TEntity> FindByKeysAsync(params object[] values);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> CreateAsync(TEntity item);
        Task BulkCreateAsync(IEnumerable<TEntity> items);

        Task UpdateAsync(TEntity item);

        Task RemoveAsync(TEntity item);
        Task RemoveRangeAsync(IEnumerable<TEntity> items);
        Task RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate);

        Task SaveAsync();
    }
}
