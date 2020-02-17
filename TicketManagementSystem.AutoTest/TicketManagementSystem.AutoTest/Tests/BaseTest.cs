using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TicketManagementSystem.AutoTest.Data.Repositories;
using TicketManagementSystem.AutoTest.Infrastructure;
using TicketManagementSystem.AutoTest.Util;

namespace TicketManagementSystem.AutoTest.Tests
{
    [TestClass]
    public abstract class BaseTest
    {
        protected static InternalTestContext _context = new InternalTestContext();

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            _context.TestOptions = OptionsHelper.GetTestOptions();

            _context.TicketRepository = new TicketRepository(_context.TestOptions.ConnectionString);

            _context.Driver = new WebDriverFactory().CreateDriver(_context.TestOptions.Browser);
            _context.Driver?.Manage().Cookies.DeleteAllCookies();
            _context.Driver?.Manage().Window.FullScreen();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _context?.Driver?.Close();
        }
    }
}
