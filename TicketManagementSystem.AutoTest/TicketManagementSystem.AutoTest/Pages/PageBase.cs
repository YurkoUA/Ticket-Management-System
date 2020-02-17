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
    public abstract class PageBase
    {
        public virtual string Url => "";
        protected readonly InternalTestContext _context;
        protected IWebDriver _driver => _context?.Driver;

        public PageBase(InternalTestContext context)
        {
            this._context = context;
        }

        #region Elements.

        public IWebElement LoginLink
        {
            get => _driver?.FindElements(By.CssSelector("body > div.navbar.navbar-inverse.navbar-fixed-top > div > div.navbar-collapse.collapse > ul.nav.navbar-nav.navbar-right > li > a"))
                .FirstOrDefault(e => e.Text == "Увійти");
        }

        public IWebElement ProfileLink
        {
            get => _driver?.FindElement(By.CssSelector("body > div.navbar.navbar-inverse.navbar-fixed-top > div > div.navbar-collapse.collapse > ul.nav.navbar-nav.navbar-right > li:nth-child(1) > a"));
        }

        public IWebElement LogoutLink
        {
            get => _driver?.FindElements(By.CssSelector("body > div.navbar.navbar-inverse.navbar-fixed-top > div > div.navbar-collapse.collapse > ul.nav.navbar-nav.navbar-right > li:nth-child(2) > a"))
                .FirstOrDefault(e => e.Text == "Вийти");
        }

        #endregion

        #region Methods.

        public void Open()
        {
            _driver.Navigate().GoToUrl(_context.TestOptions.Url + Url);
        }

        public HomePage Logout()
        {
            LogoutLink?.Click();
            return new HomePage(_context);
        }

        public LoginPage OpenProfile()
        {
            ProfileLink?.Click();
            return new LoginPage(_context);
        }

        #endregion

        #region Asserters.

        public void VerifyUserProfileLinkIsDisplayed(string userName)
        {
            var profileLink = ProfileLink;

            Assert.AreEqual(userName, profileLink?.Text, "User profile link is not displayed");
        }

        public void VerifyLoginLinkIsDisplayed()
        {
            Assert.IsNotNull(LoginLink, "Login link is not displayed");
        }

        public void VerifyLogoutLinkIsDisplayed()
        {
            Assert.IsNotNull(LogoutLink, "Logout link is not displayed");
        }

        #endregion
    }
}
