using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface IColorValidationService
    {
        IEnumerable<string> Validate(ColorCreateDTO createDTO);
        IEnumerable<string> Validate(ColorEditDTO editDTO);
    }
}
