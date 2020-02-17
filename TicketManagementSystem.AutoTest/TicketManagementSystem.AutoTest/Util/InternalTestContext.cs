using OpenQA.Selenium;

namespace TicketManagementSystem.AutoTest.Util
{
    public class InternalTestContext
    {
        public IWebDriver Driver { get; set; }
        public TestOptions TestOptions { get; set; }
    }
}
