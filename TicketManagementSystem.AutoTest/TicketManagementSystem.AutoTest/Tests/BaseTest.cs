using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TicketManagementSystem.AutoTest.Infrastructure;
using TicketManagementSystem.AutoTest.Util;

namespace TicketManagementSystem.AutoTest.Tests
{
    [TestClass]
    public abstract class BaseTest
    {
        protected static IWebDriver _driver;
        protected TestOptions _testOptions;

        [TestInitialize]
        public void Initialize()
        {
            _testOptions = OptionsHelper.GetTestOptions();
            _driver = new WebDriverFactory().CreateDriver(_testOptions.Browser);
            _driver.Manage().Cookies.DeleteAllCookies();
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            _driver?.Close();
        }
    }
}
