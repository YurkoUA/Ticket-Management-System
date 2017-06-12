namespace TicketManagementSystem.Business.DTO
{
    public class PackageSpecialCreateDTO
    {
        public string Name { get; set; }
        public int? ColorId { get; set; }
        public int? SerialId { get; set; }
        public double Nominal { get; set; }
        public string Note { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
