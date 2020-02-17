using OpenQA.Selenium;
using TicketManagementSystem.AutoTest.Util;

namespace TicketManagementSystem.AutoTest.Infrastructure
{
    public interface IWebDriverFactory
    {
        IWebDriver CreateDriver(SupportedBrowser browser);
    }
}
