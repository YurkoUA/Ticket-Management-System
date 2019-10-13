using System;

namespace TicketManagementSystem.Infrastructure.Domain.Processors
{
    public abstract class BaseProcessor
    {
        protected readonly IServiceProvider container;

        protected BaseProcessor(IServiceProvider container)
        {
            this.container = container;
        }
    }
}
