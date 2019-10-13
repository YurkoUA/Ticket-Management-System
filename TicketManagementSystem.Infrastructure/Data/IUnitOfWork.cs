using System.Threading.Tasks;

namespace TicketManagementSystem.Infrastructure.Data
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Get<TEntity>() where TEntity : class;
        Task ExecuteProcedureAsync(string name, params object[] parameters);
        Task SaveChangesAsync();
    }
}
