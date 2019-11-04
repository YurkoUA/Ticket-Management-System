namespace TicketManagementSystem.Domain.ValidationChain.Models
{
    public class PackagePropertiesChangedValidatorContext
    {
        public int PackageId { get; set; }
        public int? ColorId { get; set; }
        public int? SerialId { get; set; }
        public int? NominalId { get; set; }
    }
}
