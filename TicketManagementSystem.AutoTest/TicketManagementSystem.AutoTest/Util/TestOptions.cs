namespace TicketManagementSystem.AutoTest.Util
{
    public class TestOptions
    {
        public string Url { get; set; }
        public SupportedBrowser Browser { get; set; }

        public string ConnectionString { get; set; }

        // TODO: Move outta here.
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
