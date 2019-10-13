using System.Threading.Tasks;
using Ninject;

namespace TicketManagementSystem.Infrastructure.Domain.Processors
{
    public class QueryProcessorAsync : BaseProcessor, IQueryProcessorAsync
    {
        public QueryProcessorAsync(IKernel container) : base(container)
        {
        }

        public Task<TResponse> ProcessAsync<TResponse>(IQuery<TResponse> query)
        {
            var handlerType = typeof(IQueryHandlerAsync<,>)
                .MakeGenericType(query.GetType(), typeof(TResponse));

            dynamic handler = container.GetService(handlerType);

            return handler.GetAsync((dynamic)query);
        }
    }
}
