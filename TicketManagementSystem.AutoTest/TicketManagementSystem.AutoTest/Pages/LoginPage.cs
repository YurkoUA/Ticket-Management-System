using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace TicketManagementSystem.AutoTest.Pages
{
    public class LoginPage : PageBase
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public override string Url => "Account/Login";

        #region Elements.

        public IWebElement LoginField => _driver.FindElement(By.CssSelector("#Login"));
        public IWebElement PasswordField => _driver.FindElement(By.CssSelector("#Password"));
        public IWebElement LoginButton => _driver.FindElement(By.CssSelector("#loginGrid input.btn.btn-primary[type=submit]"));

        #endregion

        #region Methods.

        public void FillForm(string userName, string password)
        {
            LoginField.Clear();
            LoginField.SendKeys(userName);

            PasswordField.Clear();
            PasswordField.SendKeys(password);
        }

        public void Login()
        {
            LoginButton.Click();
        }

        public void Login(string userName, string password)
        {
            FillForm(userName, password);
            Login();
        }

        #endregion

        #region Asserters.

        public void VerifyBrowserUrl()
        {
            StringAssert.Contains(_driver.Url, "/Account");
        }

        #endregion
    }
}
