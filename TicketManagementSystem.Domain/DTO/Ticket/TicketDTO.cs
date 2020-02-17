namespace TicketManagementSystem.Domain.DTO.Ticket
{
    public class TicketDTO
    {
        public int? Id { get; set; }
        public string Number { get; set; }
        public int? PackageId { get; set; }
        public int ColorId { get; set; }
        public int SerialId { get; set; }
        public int NominalId { get; set; }
        public string SerialNumber { get; set; }
        public string Note { get; set; }
        public string Date { get; set; }
    }
}
