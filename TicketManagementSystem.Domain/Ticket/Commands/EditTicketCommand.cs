using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Ticket.Commands
{
    public class EditTicketCommand : ICommand<CommandResultVM<object>>
    {
        public int Id { get; set; }
        public int ColorId { get; set; }
        public int SerialId { get; set; }
        public int NominalId { get; set; }
        public string SerialNumber { get; set; }
        public string Note { get; set; }
        public string Date { get; set; }
    }
}
