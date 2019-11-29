using System.Collections.Generic;
using System.Data.Entity;
using TicketManagementSystem.Data.Entities;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class PackageFirstDigitValidator : BaseValidator
    {
        private readonly int packageId;
        private readonly int? firstDigit;

        public PackageFirstDigitValidator(IUnitOfWork unitOfWork, int packageId, int? firstDigit) : base(unitOfWork)
        {
            this.packageId = packageId;
            this.firstDigit = firstDigit;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            if (firstDigit.HasValue)
            {
                var repo = unitOfWork.Get<Data.Entities.Ticket>();
                var isEmpty = !repo.Any(t => t.PackageId == packageId);

                if (!isEmpty)
                {
                    var firstDigitStr = firstDigit.Value.ToString();
                    var supportsAll = !repo.Any(t => t.PackageId == packageId && DbFunctions.Left(t.Number, 1) != firstDigitStr);

                    if (!supportsAll)
                    {
                        model.Add(new CommandMessageDTO
                        {
                            ResourceName = ValidationMessage.PACKAGE_FIRST_DIGIT_CANNOT_BE_SET
                        });
                    }
                }
            }

            Continue(model);
        }
    }
}
