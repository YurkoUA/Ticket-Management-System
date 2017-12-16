using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface IPackageValidationService
    {
        IEnumerable<string> Validate(PackageCreateDTO createDTO);
        IEnumerable<string> Validate(PackageSpecialCreateDTO createDTO);
        IEnumerable<string> Validate(PackageEditDTO editDTO);
        IEnumerable<string> Validate(PackageSpecialEditDTO editDTO);
        IEnumerable<string> Validate(PackageMakeDefaultDTO dto);
        IEnumerable<string> Validate(PackageMakeSpecialDTO dto);
    }
}
