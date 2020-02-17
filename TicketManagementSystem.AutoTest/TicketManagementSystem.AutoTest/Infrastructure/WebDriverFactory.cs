using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TicketManagementSystem.AutoTest.Util;

namespace TicketManagementSystem.AutoTest.Infrastructure
{
    public class WebDriverFactory : IWebDriverFactory
    {
        public IWebDriver CreateDriver(SupportedBrowser browser)
        {
            switch (browser)
            {
                case SupportedBrowser.Chrome:
                    return new ChromeDriver();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
