namespace TicketManagementSystem.Domain.DTO
{
    public class CommandMessageDTO
    {
        public string ResourceName { get; set; }
        public object[] Arguments { get; set; }
    }
}
