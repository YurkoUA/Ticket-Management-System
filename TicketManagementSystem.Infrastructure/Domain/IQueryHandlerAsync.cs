using System.Threading.Tasks;

namespace TicketManagementSystem.Infrastructure.Domain
{
    public interface IQueryHandlerAsync<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        Task<TResponse> GetAsync(TQuery query);
    }
}
