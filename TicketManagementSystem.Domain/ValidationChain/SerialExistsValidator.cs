using System.Collections.Generic;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class SerialExistsValidator : BaseValidator
    {
        private int? serialId;
        private bool isRequired;
        
        public SerialExistsValidator(IUnitOfWork unitOfWork, int? serialId, bool isRequired = true) : base(unitOfWork)
        {
            this.serialId = serialId;
            this.isRequired = isRequired;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            if (isRequired || serialId.HasValue)
            {
                var isValid = unitOfWork.Get<Data.Entities.Serial>().Any(s => s.Id == serialId);

                if (!isValid)
                {
                    model.Add(new CommandMessageDTO
                    {
                        ResourceName = ValidationMessage.SERIAL_NOT_EXISTS,
                        Arguments = new object[] { serialId }
                    });
                }
            }

            Next?.HandleRequest(model);
        }
    }
}
