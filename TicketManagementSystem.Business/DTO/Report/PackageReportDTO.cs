using System.Collections.Generic;

namespace TicketManagementSystem.Business.DTO.Report
{
    public class PackageReportDTO : BaseReportDTO
    {
        public Dictionary<string, List<PackageFromReportDTO>> DefaultPackages { get; set; }
    }
}
