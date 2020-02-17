using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TicketManagementSystem.AutoTest.Util;

namespace TicketManagementSystem.AutoTest.Infrastructure
{
    public class WebDriverFactory : IWebDriverFactory
    {
        private static SupportedBrowser _browser;
        private static IWebDriver _driver;

        public IWebDriver CreateDriver(SupportedBrowser browser)
        {
            if (_browser == browser && _driver != null)
            {
                return _driver;
            }

            _browser = browser;

            switch (browser)
            {
                case SupportedBrowser.Chrome:
                    _driver = new ChromeDriver();
                    break;

                default:
                    throw new NotImplementedException();
            }

            return _driver;
        }
    }
}
