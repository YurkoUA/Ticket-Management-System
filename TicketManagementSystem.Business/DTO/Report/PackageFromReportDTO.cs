namespace TicketManagementSystem.Business.DTO.Report
{
    public class PackageFromReportDTO
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public int TotalTickets { get; set; }
        public int NewTickets { get; set; }

        public string TrClass()
        {
            if (NewTickets > 0)
                return "active";

            return null;
        }
    }
}
