namespace TicketManagementSystem.Business.DTO
{
    public class PackageEditDTO
    {
        public int Id { get; set; }

        public int ColorId { get; set; }
        public int SerialId { get; set; }
        public int? FirstNumber { get; set; }
        public double Nominal { get; set; }
        public string Note { get; set; }

        public int TicketsCount { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
