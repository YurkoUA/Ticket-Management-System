using System.Collections.Generic;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class ColorExistsValidator : BaseValidator
    {
        private readonly int? colorId;
        private readonly bool isRequired;

        public ColorExistsValidator(IUnitOfWork unitOfWork, int? colorId, bool isRequired = true) : base(unitOfWork)
        {
            this.colorId = colorId;
            this.isRequired = isRequired;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            if (isRequired || colorId.HasValue)
            {
                var isValid = unitOfWork.Get<Data.Entities.Color>().Any(c => c.Id == colorId);

                if (!isValid)
                {
                    model.Add(new CommandMessageDTO
                    {
                        ResourceName = ValidationMessage.COLOR_NOT_EXISTS,
                        Arguments = new object[] { colorId }
                    });
                }
            }

            Continue(model);
        }
    }
}
