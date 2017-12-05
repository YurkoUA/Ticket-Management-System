using System;
using System.Collections.Generic;

namespace TicketManagementSystem.Business.DTO.Report
{
    public abstract class BaseReportDTO
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public DateTime? LastReportDate { get; set; }

        // Tickets.
        public int TicketsCount { get; set; }
        public int HappyTicketsCount { get; set; }
        public int NewTicketsCount { get; set; }
        public int NewHappyTicketsCount { get; set; }

        // Packages.
        public int PackagesCount => DefaultPackagesCount + SpecialPackagesCount;
        public int DefaultPackagesCount { get; set; }
        public int SpecialPackagesCount { get; set; }
        public int NewPackagesCount { get; set; }

        public List<PackageFromReportDTO> SpecialPackages { get; set; }
    }
}
