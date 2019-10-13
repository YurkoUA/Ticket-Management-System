using System.Threading.Tasks;

namespace TicketManagementSystem.Infrastructure.Domain.Processors
{
    public interface IQueryProcessorAsync
    {
        Task<TResponse> ProcessAsync<TResponse>(IQuery<TResponse> query);
    }
}
