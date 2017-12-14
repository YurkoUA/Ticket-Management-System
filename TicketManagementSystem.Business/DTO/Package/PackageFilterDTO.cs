using TicketManagementSystem.Business.Enums;

namespace TicketManagementSystem.Business.DTO
{
    public class PackageFilterDTO
    {
        public int? ColorId { get; set; }
        public int? SerialId { get; set; }
        public int? FirstNumber { get; set; }
        public PackageStatusFilter Status { get; set; }
    }
}
