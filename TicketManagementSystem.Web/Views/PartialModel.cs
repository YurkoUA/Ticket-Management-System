namespace TicketManagementSystem.Web
{
    public class PartialModel<T>
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public T Param { get; set; }
    }
}