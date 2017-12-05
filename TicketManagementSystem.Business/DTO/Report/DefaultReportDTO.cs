using System.Collections.Generic;

namespace TicketManagementSystem.Business.DTO.Report
{
    public class DefaultReportDTO : BaseReportDTO
    {
        public int UnallocatedTicketsCount { get; set; }
        public int NewUnallocatedTicketsCount { get; set; }

        public int DefaultPackagesTickets { get; set; }
        public int NewDefaultPackagesTickets { get; set; }
        
        public List<TicketGroupDTO> NewTicketsGroups { get; set; }
    }
}
