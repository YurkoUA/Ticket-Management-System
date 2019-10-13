using Ninject;

namespace TicketManagementSystem.Infrastructure.Domain.Processors
{
    public abstract class BaseProcessor
    {
        protected readonly IKernel container;

        protected BaseProcessor(IKernel container)
        {
            this.container = container;
        }
    }
}
