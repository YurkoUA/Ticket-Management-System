using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Package.Commands
{
    public class EditPackageCommand : ICommand<CommandResultVM<object>>
    {
        public int Id { get; set; }
        public int ColorId { get; set; }
        public int SerialId { get; set; }
        public int NominalId { get; set; }
        public int? FirstDigit { get; set; }
        public string Note { get; set; }
    }
}
