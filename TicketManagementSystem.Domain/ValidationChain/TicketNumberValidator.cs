using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.Ticket.Commands;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class TicketNumberValidator : BaseValidator
    {
        public TicketNumberValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {

        }
    }
}
