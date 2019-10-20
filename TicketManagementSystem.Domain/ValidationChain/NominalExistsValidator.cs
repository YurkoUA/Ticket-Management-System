using System.Collections.Generic;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class NominalExistsValidator : BaseValidator
    {
        private readonly int? nominalId;
        private readonly bool isRequired;

        public NominalExistsValidator(IUnitOfWork unitOfWork, int? nominalId, bool isRequired = true) : base(unitOfWork)
        {
            this.nominalId = nominalId;
            this.isRequired = isRequired;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            if (isRequired || nominalId.HasValue)
            {
                var isValid = unitOfWork.Get<Data.Entities.Nominal>().Any(n => n.Id == nominalId);

                if (!isValid)
                {
                    model.Add(new CommandMessageDTO
                    {
                        ResourceName = ValidationMessage.NOMINAL_NOT_EXISTS,
                        Arguments = new object[] { nominalId }
                    });
                }
            }

            Continue(model);
        }
    }
}
