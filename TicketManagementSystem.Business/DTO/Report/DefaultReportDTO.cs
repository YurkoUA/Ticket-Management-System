using System;
using System.Collections.Generic;

namespace TicketManagementSystem.Business.DTO.Report
{
    public class DefaultReportDTO
    {
        public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime? LastReportDate { get; set; }

        public int TicketsCount { get; set; }
        public int HappyTicketsCount { get; set; }
        public int NewTicketsCount { get; set; }
        public int NewHappyTicketsCount { get; set; }

        public int DefaultPackagesCount { get; set; }
        public int SpecialPackagesCount { get; set; }

        public int PackagesCount => DefaultPackagesCount + SpecialPackagesCount;
        public int NewPackagesCount { get; set; }

        public int UnallocatedTicketsCount { get; set; }
        public int NewUnallocatedTicketsCount { get; set; }
        public int DefaultPackagesTickets { get; set; }
        public int DefaultPackagesTicketsNew { get; set; }

        public List<PackageDTO> Packages { get; set; }
        public List<TicketsGroup> NewTicketsGroups { get; set; }
    }
}
