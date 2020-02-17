using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.DTO.Ticket;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class PackageSupportsTicketValidator : BaseValidator
    {
        private readonly TicketDTO ticket;

        public PackageSupportsTicketValidator(IUnitOfWork unitOfWork, TicketDTO ticket) : base(unitOfWork)
        {
            this.ticket = ticket;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            var package = unitOfWork.Get<Data.Entities.Package>().Find(ticket.PackageId.Value);

            if (package.NominalId != ticket.NominalId
                || package.ColorId != ticket.ColorId
                || package.SerialId != ticket.SerialId)
            {
                AddError(model, ValidationMessage.PACKAGE_NOM_COL_SER_ARE_DIFF_FROM_TICKET);
            }

            if (package.FirstNumber.HasValue)
            {
                var ticketFirstDigit = int.Parse(ticket.Number.First().ToString());

                if (package.FirstNumber != ticketFirstDigit)
                {
                    AddError(model, ValidationMessage.PACKAGE_FIRST_DIGIT_DIFF_FROM_TICKET);
                }
            }

            Continue(model);
        }
    }
}
