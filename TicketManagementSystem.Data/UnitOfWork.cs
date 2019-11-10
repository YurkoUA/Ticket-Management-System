using System.Collections.Generic;
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
            await context.Database.ExecuteSqlCommandAsync($"EXEC {name}", parameters);
        }

        public async Task<IEnumerable<T>> ExecuteProcedureAsync<T>(string name, params object[] parameters)
        {
            return await context.Database.SqlQuery<T>($"EXEC {name}", parameters).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
