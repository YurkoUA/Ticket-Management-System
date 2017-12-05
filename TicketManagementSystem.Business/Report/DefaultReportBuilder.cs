using System.Collections.Generic;
using TicketManagementSystem.Business.DTO.Report;

namespace TicketManagementSystem.Business.Report
{
    public class DefaultReportBuilder : ReportBuilder<DefaultReportDTO, DefaultReportBuilder>
    {
        public DefaultReportBuilder()
        {
            _reportDto = new DefaultReportDTO();
        }

        public DefaultReportBuilder SetUnallocatedTicketsCount(int count)
        {
            _reportDto.UnallocatedTicketsCount = count;
            return this;
        }

        public DefaultReportBuilder SetDefaultPackagesTickets(int count)
        {
            _reportDto.DefaultPackagesTickets = count;
            return this;
        }

        public DefaultReportBuilder SetNewUnallocatedTicketsCount(int count)
        {
            _reportDto.NewUnallocatedTicketsCount = count;
            return this;
        }

        public DefaultReportBuilder SetNewDefaultPackagesTickets(int count)
        {
            _reportDto.NewDefaultPackagesTickets = count;
            return this;
        }

        public DefaultReportBuilder SetNewTicketsGroups(List<TicketGroupDTO> ticketGroups)
        {
            _reportDto.NewTicketsGroups = ticketGroups;
            return this;
        }
    }
}
