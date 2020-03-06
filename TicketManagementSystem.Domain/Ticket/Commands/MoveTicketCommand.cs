using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Ticket.Commands
{
    public class MoveTicketCommand : ICommand<CommandResultVM<object>>
    {
        public int TicketId { get; set; }
        public int? PackageId { get; set; }
    }
}
