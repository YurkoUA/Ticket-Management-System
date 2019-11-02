using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public class PackageColorValidator : BaseValidator
    {
        public PackageColorValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override void HandleRequest(IList<CommandMessageDTO> model)
        {
            throw new NotImplementedException();
        }
    }
}
