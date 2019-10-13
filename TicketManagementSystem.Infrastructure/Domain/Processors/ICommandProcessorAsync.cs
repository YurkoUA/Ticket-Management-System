using System.Threading.Tasks;

namespace TicketManagementSystem.Infrastructure.Domain.Processors
{
    public interface ICommandProcessorAsync
    {
        Task ProcessAsync(ICommand command);
        Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command);
    }
}
