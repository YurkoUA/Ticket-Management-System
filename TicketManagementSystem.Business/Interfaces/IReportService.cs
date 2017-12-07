using System;
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

        bool TryCreatePDFs(ReportDTO reportDTO, Func<string, string> actionUrl, Func<string, string> savePath);

        DefaultReportDTO GetDefaultReportDTO();
        PackageReportDTO GetPackagesReportDTO();
    }
}
