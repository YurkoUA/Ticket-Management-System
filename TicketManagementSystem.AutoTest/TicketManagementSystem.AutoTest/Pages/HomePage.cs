using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TicketManagementSystem.AutoTest.Util;

namespace TicketManagementSystem.AutoTest.Pages
{
    public class HomePage : PageBase
    {
        public HomePage(InternalTestContext context) : base(context)
        {
        }

        public LoginPage OpenLoginPage()
        {
            LoginLink.Click();
            return new LoginPage(_context);
        }

        #region Asserters.



        #endregion
    }
}
