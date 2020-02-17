using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class PackageStateValidator : BaseValidator
    {
        private readonly int packageId;
        private readonly bool isOpened;

        public PackageStateValidator(IUnitOfWork unitOfWork, int packageId, bool isOpened = true) : base(unitOfWork)
        {
            this.packageId = packageId;
            this.isOpened = isOpened;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            var package = unitOfWork.Get<Data.Entities.Package>().Find(packageId);
            var isValid = package.IsOpened == isOpened;

            if (!isValid)
            {
                AddError(model, isOpened ? ValidationMessage.PACKAGE_CLOSED : ValidationMessage.PACKAGE_OPENED);
            }

            Continue(model);
        }
    }
}
