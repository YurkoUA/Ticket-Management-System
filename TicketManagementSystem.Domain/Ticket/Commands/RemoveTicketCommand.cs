using TicketManagementSystem.Infrastructure.Domain;

namespace TicketManagementSystem.Domain.Ticket.Commands
{
    public class RemoveTicketCommand : ICommand
    {
        public int TicketId { get; set; }
    }
}
