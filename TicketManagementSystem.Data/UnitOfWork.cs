using System.Threading.Tasks;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly AppDbContext context;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public IRepository<TEntity> Get<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(context);
        }

        public async Task ExecuteProcedureAsync(string name, params object[] parameters)
        {
            await context.Database.ExecuteSqlCommandAsync(name, parameters);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
