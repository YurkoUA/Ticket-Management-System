using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.DTO.Ticket;
using TicketManagementSystem.Domain.ValidationChain.Models;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class TicketNumberValidator : BaseValidator
    {
        private readonly TicketDTO ticket;

        public TicketNumberValidator(IUnitOfWork unitOfWork, TicketDTO ticket) : base(unitOfWork)
        {
            this.ticket = ticket;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            Expression<Func<Data.Entities.Ticket, bool>> expression = null;

            if (ticket.Id.HasValue)
            {
                expression = t => t.Number == ticket.Number
                    && t.NominalId == ticket.NominalId
                    && t.ColorId == ticket.ColorId
                    && t.SerialId == ticket.SerialId
                    && t.SerialNumber == ticket.SerialNumber
                    && t.Id != ticket.Id.Value;
            }
            else
            {
                expression = t => t.Number == ticket.Number 
                    && t.NominalId == ticket.NominalId
                    && t.ColorId == ticket.ColorId
                    && t.SerialId == ticket.SerialId
                    && t.SerialNumber == ticket.SerialNumber;
            }

            var isValid = !unitOfWork.Get<Data.Entities.Ticket>().Any(expression);

            if (!isValid)
            {
                model.Add(new CommandMessageDTO
                {
                    ResourceName = ValidationMessage.TICKET_ALREADY_EXISTS,
                    Arguments = new object[] { ticket.Number }
                });
            }

            Continue(model);
        }
    }
}
