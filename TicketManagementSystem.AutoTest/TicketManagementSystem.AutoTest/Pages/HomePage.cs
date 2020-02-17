using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace TicketManagementSystem.AutoTest.Pages
{
    public class HomePage : PageBase
    {
        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        public LoginPage OpenLoginPage()
        {
            LoginLink.Click();
            return new LoginPage(_driver);
        }

        public void Logout()
        {
            LogoutLink.Click();
        }

        public void OpenProfile()
        {
            ProfileLink.Click();
        }

        #region Asserters.



        #endregion
    }
}
