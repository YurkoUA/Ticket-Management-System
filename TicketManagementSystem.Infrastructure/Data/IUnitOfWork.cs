using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace TicketManagementSystem.Infrastructure.Data
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Get<TEntity>() where TEntity : class;
        Task ExecuteProcedureAsync(string name, IEnumerable<IDbDataParameter> parameters);
        Task<IEnumerable<T>> ExecuteProcedureAsync<T>(string name, IEnumerable<IDbDataParameter> parameters);
        Task SaveChangesAsync();
    }
}
