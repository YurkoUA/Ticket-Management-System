namespace TicketManagementSystem.Business.DTO.Report
{
    public class PackageDTO
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public int TotalTickets { get; set; }
        public int NewTickets { get; set; }
    }
}
