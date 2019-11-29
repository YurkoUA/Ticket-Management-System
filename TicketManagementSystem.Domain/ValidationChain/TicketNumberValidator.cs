using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.ValidationChain.Models;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class TicketNumberValidator : BaseValidator
    {
        private readonly TicketNumberValidatorContext context;

        public TicketNumberValidator(IUnitOfWork unitOfWork, TicketNumberValidatorContext context) : base(unitOfWork)
        {
            this.context = context;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            Expression<Func<Data.Entities.Ticket, bool>> expression = null;

            if (context.Id.HasValue)
            {
                expression = t => t.Number == context.Number
                    && t.NominalId == context.NominalId
                    && t.ColorId == context.ColorId
                    && t.SerialId == context.SerialId
                    && t.SerialNumber == context.SerialNumber
                    && t.Id != context.Id.Value;
            }
            else
            {
                expression = t => t.Number == context.Number 
                    && t.NominalId == context.NominalId
                    && t.ColorId == context.ColorId
                    && t.SerialId == context.SerialId
                    && t.SerialNumber == context.SerialNumber;
            }

            var isValid = !unitOfWork.Get<Data.Entities.Ticket>().Any(expression);

            if (!isValid)
            {
                model.Add(new CommandMessageDTO
                {
                    ResourceName = ValidationMessage.TICKET_ALREADY_EXISTS,
                    Arguments = new object[] { context.Number }
                });
            }
        }
    }
}
