using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ISummaryService
    {
        IEnumerable<SummaryDTO> GetSummaries();
        IEnumerable<SummaryPeriodDTO> GetSummariesPeriods();
        void WriteSummary();
        bool ExistsByCurrentMonth();
    }
}
