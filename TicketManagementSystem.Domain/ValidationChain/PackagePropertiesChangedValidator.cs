using System.Collections.Generic;
using TicketManagementSystem.Data.Entities;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.Package.Commands;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class PackagePropertiesChangedValidator : BaseValidator
    {
        private readonly EditPackageCommand command;

        public PackagePropertiesChangedValidator(IUnitOfWork unitOfWork, EditPackageCommand command) : base(unitOfWork)
        {
            this.command = command;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            var ticketRepo = unitOfWork.Get<Ticket>();
            var hasTickets = ticketRepo.Any(t => t.PackageId == command.Id);

            if (hasTickets)
            {
                var package = unitOfWork.Get<Data.Entities.Package>().Find(command.Id);

                if (command.ColorId != package.ColorId || command.SerialId != package.SerialId || command.NominalId != package.NominalId)
                {
                    model.Add(new CommandMessageDTO
                    {
                        ResourceName = ValidationMessage.PACKAGE_PROPERTIES_DIFFERENT
                    });
                    return;
                }
            }

            Continue(model);
        }
    }
}
