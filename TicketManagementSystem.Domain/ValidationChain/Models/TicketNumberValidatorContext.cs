namespace TicketManagementSystem.Domain.ValidationChain.Models
{
    public class TicketNumberValidatorContext
    {
        public int? Id { get; set; }
        public string Number { get; set; }
        public int NominalId { get; set; }
        public int ColorId { get; set; }
        public int SerialId { get; set; }
        public string SerialNumber { get; set; }
    }
}
