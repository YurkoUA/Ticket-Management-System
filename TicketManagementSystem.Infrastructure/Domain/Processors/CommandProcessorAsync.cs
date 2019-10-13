using System;
using System.Threading.Tasks;

namespace TicketManagementSystem.Infrastructure.Domain.Processors
{
    public class CommandProcessorAsync : BaseProcessor, ICommandProcessorAsync
    {
        public CommandProcessorAsync(IServiceProvider container) : base(container)
        {
        }

        public Task ProcessAsync(ICommand command)
        {
            var handlerType = typeof(ICommandHandlerAsync<>)
                .MakeGenericType(command.GetType());

            dynamic handler = container.GetService(handlerType);

            return handler.ExecuteAsync((dynamic)command);
        }

        public Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command)
        {
            var handlerType = typeof(ICommandHandlerAsync<,>)
                .MakeGenericType(command.GetType(), typeof(TResult));

            dynamic handler = container.GetService(handlerType);

            return handler.ExecuteAsync((dynamic)command);
        }
    }
}
