using System;
using System.Collections.Generic;

namespace TicketManagementSystem.Business.DTO.Report
{
    public class PackageReportDTO
    {
        public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime? LastReportDate { get; set; }

        public int TotalTickets { get; set; }
        public int NewTickets { get; set; }
        public int HappyTickets { get; set; }
        public List<PackageDTO> DefaultPackages { get; set; }
        public List<PackageDTO> SpecialPackages { get; set; }

        public int TotalPackages { get; set; }
        public int NewPackagesCount { get; set; }
    }
}
