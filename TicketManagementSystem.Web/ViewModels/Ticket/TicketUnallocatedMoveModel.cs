namespace TicketManagementSystem.Web
{
    public class TicketUnallocatedMoveModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string ColorName { get; set; }
        public string SerialFull { get; set; }
        public bool Move { get; set; }
        public bool IsHappy { get; set; }

        public override string ToString()
        {
            return $"{Number} ({ColorName}, {SerialFull})";
        }
    }
}