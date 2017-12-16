using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ITodoValidationService
    {
        IEnumerable<string> ValidateCreate(TodoTaskDTO dto);
        IEnumerable<string> ValidateEdit(TodoTaskDTO dto);
    }
}
