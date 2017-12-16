using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ISerialValidationService
    {
        IEnumerable<string> Validate(SerialCreateDTO createDTO);
        IEnumerable<string> Validate(SerialEditDTO editDTO);
    }
}
