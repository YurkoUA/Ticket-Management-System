using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Package.Commands
{
    public class CreatePackageCommand : ICommand<CommandResultVM<IdentifierVM>>
    {
        public int ColorId { get; set; }
        public int SerialId { get; set; }
        public int NominalId { get; set; }
        public int? FirstDigit { get; set; }
        public string Note { get; set; }
    }
}
