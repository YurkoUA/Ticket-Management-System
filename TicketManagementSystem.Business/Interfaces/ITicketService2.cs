using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ITicketService2
    {
        IEnumerable<TicketDTO> GetLatestTickets();
    }
}
