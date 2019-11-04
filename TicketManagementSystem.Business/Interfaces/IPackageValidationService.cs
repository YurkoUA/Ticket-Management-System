using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface IPackageValidationService
    {
        IEnumerable<string> Validate(PackageMakeDefaultDTO dto);
        IEnumerable<string> Validate(PackageMakeSpecialDTO dto);
    }
}
