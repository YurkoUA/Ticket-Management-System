using System;
using System.Collections.Generic;
using System.Linq;
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
                var repo = unitOfWork.Get<Ticket>();
                var isEmpty = !repo.Any(t => t.PackageId == packageId);

                if (!isEmpty)
                {
                    var supportsAll = !repo.Any(t => Convert.ToInt32(t.Number.First()) != firstDigit.Value);

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
