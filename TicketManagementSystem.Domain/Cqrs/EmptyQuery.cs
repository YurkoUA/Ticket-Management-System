using TicketManagementSystem.Infrastructure.Domain;

namespace TicketManagementSystem.Domain.Cqrs
{
    public class EmptyQuery<TResponse> : IQuery<TResponse>
    {
    }
}
