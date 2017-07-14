using System;

namespace TicketManagementSystem.Business.DTO.Report
{
    public class ReportDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsAutomatic { get; set; }

        public string TrClass => !IsAutomatic ? "warning" : "";

        public string DefaultReportFileName()
        {
            return $"Report_Default_{ConvertDateTime(Date)}";
        }

        public string PackagesReportFileName()
        {
            return $"Report_Packages_{ConvertDateTime(Date)}";
        }

        private string ConvertDateTime(DateTime date)
        {
            return date.ToString("s").Replace("T", "_").Replace(":", "");
        }
    }
}
