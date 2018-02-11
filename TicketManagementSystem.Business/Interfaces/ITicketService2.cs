using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.DTO.Report;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ITicketService2
    {
        IEnumerable<TicketDTO> GetLatestTickets();
        IEnumerable<TicketDTO> GetTodayTickets(int timezoneOffset);
        IEnumerable<TicketGroupDTO> GetSummaryByLatest();
        IEnumerable<TicketGroupDTO> GetSummaryByUnallocated();

        IEnumerable<string> GetNotes(string note, int take);
    }
}
