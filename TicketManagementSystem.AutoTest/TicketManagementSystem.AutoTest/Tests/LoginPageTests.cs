using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagementSystem.AutoTest.Infrastructure;
using TicketManagementSystem.AutoTest.Pages;
using TicketManagementSystem.AutoTest.Util;

namespace TicketManagementSystem.AutoTest.Tests
{
    [TestClass]
    public class LoginPageTests : BaseTest
    {
        [TestMethod]
        [Description("User logs in and logs out")]
        public void Login_Logout_Are_PerformedSuccessfully()
        {
            var homePage = new HomePage(_context);
            homePage.Open();

            homePage.VerifyLoginLinkIsDisplayed();

            var loginPage = homePage.OpenLoginPage();
            loginPage.Login(_context.TestOptions.Login, _context.TestOptions.Password);

            Thread.Sleep(1000);

            loginPage.VerifyBrowserUrl();
            loginPage.VerifyUserProfileLinkIsDisplayed(_context.TestOptions.Login);
            loginPage.VerifyLogoutLinkIsDisplayed();

            loginPage.Logout();

            Thread.Sleep(1000);

            loginPage.VerifyLoginLinkIsDisplayed();
        }
    }
}
