using System.Collections.Generic;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class PackageExistsValidator : BaseValidator
    {
        private readonly int? packageId;
        private readonly bool isRequired;

        public PackageExistsValidator(IUnitOfWork unitOfWork, int? packageId, bool isRequired = true) : base(unitOfWork)
        {
            this.packageId = packageId;
            this.isRequired = isRequired;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            if (isRequired || packageId.HasValue)
            {
                var isValid = unitOfWork.Get<Data.Entities.Package>().Any(p => p.Id == packageId);

                if (!isValid)
                {
                    model.Add(new CommandMessageDTO
                    {
                        ResourceName = ValidationMessage.PACKAGE_NOT_EXISTS,
                        Arguments = new object[] { packageId }
                    });
                }
            }

            Continue(model);
        }
    }
}
