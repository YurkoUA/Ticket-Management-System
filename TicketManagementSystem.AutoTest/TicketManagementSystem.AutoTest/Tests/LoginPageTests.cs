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
    public class LoginPageTests
    {
        [TestMethod]
        public void LoginPerformedSuccessfully()
        {
            var url = "http://localhost:55557/";
            var browser = SupportedBrowser.Chrome;

            var factory = new WebDriverFactory();
            var driver = factory.CreateDriver(browser);

            driver.Navigate().GoToUrl(url);

            var homePage = new HomePage(driver);

            homePage.VerifyLoginLinkIsDisplayed();

            var loginPage = homePage.OpenLoginPage();
            loginPage.FillForm("admin", "AutoTest");
            loginPage.Login();

            Thread.Sleep(1000);

            loginPage.VerifyBrowserUrl();
            loginPage.VerifyUserProfileLinkIsDisplayed("admin");
            loginPage.VerifyLogoutLinkIsDisplayed();

            driver.Close();
        }
    }
}
