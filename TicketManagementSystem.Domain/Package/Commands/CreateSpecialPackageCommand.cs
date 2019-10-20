using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Package.Commands
{
    public class CreateSpecialPackageCommand : ICommand<CommandResultVM<IdentifierVM>>
    {
        public string Name { get; set; }
        public int? ColorId { get; set; }
        public int? SerialId { get; set; }
        public int? NominalId { get; set; }
        public string Note { get; set; }
    }
}
