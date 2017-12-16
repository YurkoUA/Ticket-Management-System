using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ITicketValidationService
    {
        IEnumerable<string> Validate(TicketCreateDTO createDTO);
        IEnumerable<string> Validate(TicketEditDTO editDTO);
        IEnumerable<string> ValidateChangeNumber(int ticketId, string newNumber);
        IEnumerable<string> ValidateMoveToPackage(int ticketId, int packageId);
        IEnumerable<string> ValidateMoveFewToPackage(int packageId, params int[] ticketsIds);
    }
}
