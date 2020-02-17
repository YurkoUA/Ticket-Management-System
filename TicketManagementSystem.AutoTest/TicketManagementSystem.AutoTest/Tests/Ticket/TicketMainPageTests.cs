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
            var loginPage = new LoginPage(_driver);
            loginPage.Open(_testOptions.Url);
            loginPage.Login(_testOptions.Login, _testOptions.Password);

            Thread.Sleep(1000);

            page = new TicketsMainPage(_driver);
            page.Open(_testOptions.Url);
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
