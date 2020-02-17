using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagementSystem.AutoTest.Pages;
using TicketManagementSystem.AutoTest.Pages.Ticket;
using TicketManagementSystem.AutoTest.Pages.Ticket.Modal;

namespace TicketManagementSystem.AutoTest.Tests.Ticket
{
    [TestClass]
    public class TicketSearchModalTests : BaseTest
    {
        private static TicketsMainPage page;
        private static TicketSearchModal modal;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            var loginPage = new LoginPage(_context);
            loginPage.Open();
            loginPage.Login(_context.TestOptions.Login, _context.TestOptions.Password);

            Thread.Sleep(1000);

            page = new TicketsMainPage(_context);
            page.Open();
            modal = page.OpenTicketSearch();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            modal?.Close();
            page?.Logout();
        }

        [TestMethod]
        [Description("User opens ticket search and searches for a ticket")]
        public void TicketSearchModal_SearchTicket()
        {
            var ticket = _context.TicketRepository.GetRandomTicket();
            var ticketNumber = ticket.Number;

            // Search by full number.
            modal.Search(ticketNumber);
            Thread.Sleep(1000);

            modal.VerifyResultContainsTicket(ticket.Number);

            // Search by 4 symbols in beginning.
            ticketNumber = ticket.Number.Substring(0, 4);
            modal.Search(ticketNumber);
            Thread.Sleep(1000);

            modal.VerifyResultContainsTicket(ticket.Number);

            // Search by 4 symbols the end.
            ticketNumber = ticket.Number.Substring(2, 4);
            modal.Search(ticketNumber);
            Thread.Sleep(1000);

            modal.VerifyResultContainsTicket(ticket.Number);

            // Search by symbols in the middle.
            ticketNumber = ticket.Number.Substring(1, 4);
            modal.Search(ticketNumber);
            Thread.Sleep(1000);

            modal.VerifyResultContainsTicket(ticket.Number);
        }
    }
}
