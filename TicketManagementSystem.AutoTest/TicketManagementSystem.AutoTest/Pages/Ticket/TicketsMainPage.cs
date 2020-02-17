using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TicketManagementSystem.AutoTest.Util;

namespace TicketManagementSystem.AutoTest.Pages.Ticket
{
    public class TicketsMainPage : PageBase
    {
        public override string Url => "Ticket";

        public TicketsMainPage(InternalTestContext context) : base(context)
        {
        }

        #region Elements.

        public IWebElement SearchTicketsLink => _driver.FindElement(By.Id("ticket-search"));

        #endregion

        #region Methods.

        public void OpenTicketSearch()
        {
            SearchTicketsLink?.Click();
        }

        #endregion
    }
}
