using System.Threading.Tasks;

namespace TicketManagementSystem.Infrastructure.Domain
{
    public interface ICommandHandlerAsync<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> ExecuteAsync(TCommand command);
    }

    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }
}
