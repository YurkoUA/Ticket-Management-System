using System.Collections.Generic;
using TicketManagementSystem.Business.DTO.Report;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface IReportService
    {
        bool IsEmpty { get; }

        IEnumerable<ReportDTO> GetReports();
        ReportDTO GetById(int id);
        ReportDTO GetLastReport();

        ReportDTO CreateReport(bool isAutomatic);
        void SaveReport(ReportDTO reportDTO);

        DefaultReportDTO GetDefaultReportDTO();
        PackageReportDTO GetPackagesReportDTO();
    }
}
