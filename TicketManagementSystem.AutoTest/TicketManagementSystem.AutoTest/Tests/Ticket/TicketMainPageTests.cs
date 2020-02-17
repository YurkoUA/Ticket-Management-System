using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagementSystem.AutoTest.Pages;
using TicketManagementSystem.AutoTest.Pages.Ticket;

namespace TicketManagementSystem.AutoTest.Tests.Ticket
{
    [TestClass]
    public class TicketMainPageTests : BaseTest
    {
        private static TicketsMainPage page;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            var loginPage = new LoginPage(_context);
            loginPage.Open();
            loginPage.Login(_context.TestOptions.Login, _context.TestOptions.Password);

            Thread.Sleep(1000);

            page = new TicketsMainPage(_context);
            page.Open();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            page.Logout();
        }

        [TestMethod]
        [Description("User opens ticket search and searches for a ticket")]
        public void TicketSearchModal_Show()
        {
            page.OpenTicketSearch();
            // TODO: To be implemented.
        }
    }
}
