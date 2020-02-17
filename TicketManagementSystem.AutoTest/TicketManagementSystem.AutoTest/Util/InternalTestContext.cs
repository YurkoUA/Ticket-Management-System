using OpenQA.Selenium;
using TicketManagementSystem.AutoTest.Data.Repositories;

namespace TicketManagementSystem.AutoTest.Util
{
    public class InternalTestContext
    {
        public IWebDriver Driver { get; set; }
        public TestOptions TestOptions { get; set; }

        // TODO: Move to UnitOfWork or smth else.
        public TicketRepository TicketRepository { get; set; }
    }
}
