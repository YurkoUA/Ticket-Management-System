using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketManagementSystem.Business.DTO.Report;

namespace TicketManagementSystem.Web
{
    public class ReportIndexModel
    {
        public ReportDTO LastReport { get; set; }
        public bool ArchiveIsEmpty { get; set; }
    }
}