using System.Collections.Generic;
using TicketManagementSystem.Data.Entities;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.Package.Commands;
using TicketManagementSystem.Domain.ValidationChain.Models;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class PackagePropertiesChangedValidator : BaseValidator
    {
        private readonly PackagePropertiesChangedValidatorContext context;

        public PackagePropertiesChangedValidator(IUnitOfWork unitOfWork, PackagePropertiesChangedValidatorContext context) : base(unitOfWork)
        {
            this.context = context;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            var ticketRepo = unitOfWork.Get<Data.Entities.Ticket>();
            var hasTickets = ticketRepo.Any(t => t.PackageId == context.PackageId);

            if (hasTickets)
            {
                var package = unitOfWork.Get<Data.Entities.Package>().Find(context.PackageId);

                if ((context.ColorId.HasValue && context.ColorId != package.ColorId) 
                    || (context.SerialId.HasValue && context.SerialId != package.SerialId) 
                    || (context.NominalId.HasValue && context.NominalId != package.NominalId))
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
