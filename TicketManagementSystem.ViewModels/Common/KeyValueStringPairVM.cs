namespace TicketManagementSystem.ViewModels.Common
{
    public class KeyValueStringPairVM
    {
        public KeyValueStringPairVM(int key, string value)
        {
            Key = key;
            Value = value;
        }

        public int Key { get; set; }
        public string Value { get; set; }
    }
}
