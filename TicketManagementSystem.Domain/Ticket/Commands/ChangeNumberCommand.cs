using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Ticket.Commands
{
    public class ChangeNumberCommand : ICommand<CommandResultVM<object>>
    {
        public int Id { get; set; }
        public string Number { get; set; }
    }
}
