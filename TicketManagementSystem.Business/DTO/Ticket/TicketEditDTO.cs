namespace TicketManagementSystem.Business.DTO
{
    public class TicketEditDTO
    {
        public byte[] RowVersion { get; set; }      
        public int Id { get; set; }        
        public int ColorId { get; set; }        
        public int SerialId { get; set; }        
        public string SerialNumber { get; set; }        
        public string Note { get; set; }
        public string Date { get; set; }
    }
}
