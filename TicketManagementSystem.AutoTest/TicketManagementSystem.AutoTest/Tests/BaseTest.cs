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
        protected static TestOptions _testOptions;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            _testOptions = OptionsHelper.GetTestOptions();
            _driver = new WebDriverFactory().CreateDriver(_testOptions.Browser);
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.FullScreen();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _driver?.Close();
        }
    }
}
