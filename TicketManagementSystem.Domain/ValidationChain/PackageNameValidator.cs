using System.Collections.Generic;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class PackageNameValidator : BaseValidator
    {
        private readonly string name;
        private readonly int? id;

        public PackageNameValidator(IUnitOfWork unitOfWork, string name, int? id = null) : base(unitOfWork)
        {
            this.name = name;
            this.id = id;
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            var isValid = !unitOfWork.Get<Data.Entities.Package>().Any(p => p.Name == name && (id == null || p.Id != id));

            if (!isValid)
            {
                model.Add(new CommandMessageDTO
                {
                    ResourceName = ValidationMessage.PACKAGE_NAME_EXISTS,
                    Arguments = new object[] { name }
                });
            }

            Continue(model);
        }
    }
}
