namespace TicketManagementSystem.Domain.Package.Queries
{
    public class GetCompatiblePackagesQuery
    {
        public int? FirstDigit { get; set; }
        public int ColorId { get; set; }
        public int SerialId { get; set; }
        public int NominalId { get; set; }
    }
}
