using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public async Task ExecuteProcedureAsync(string name, IEnumerable<IDbDataParameter> parameters)
        {
            var paramsNames = parameters.Select(p => $"@{p.ParameterName} ");
            await context.Database.ExecuteSqlCommandAsync($"EXEC {name} {paramsNames}", parameters);
        }

        public async Task<IEnumerable<T>> ExecuteProcedureAsync<T>(string name, IEnumerable<IDbDataParameter> parameters)
        {
            var paramsNames = string.Join(" ", parameters.Select(p => $"@{p.ParameterName}"));
            return await context.Database.SqlQuery<T>($"EXEC {name} {paramsNames}", parameters.ToArray()).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
