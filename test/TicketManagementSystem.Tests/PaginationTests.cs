using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagementSystem.Web;
using TicketManagementSystem.Web.Data;

namespace TicketManagementSystem.Tests
{
    [TestClass]
    [TestCategory("PaginationTests")]
    public class PaginationTests
    {
        private const int BUTTONS_ON_PAGE = 7;
        private const int PAGE_SIZE = 3;
        private const int TOTAL_ITEMS = 25;

        [TestMethod]
        public void PaginationInterval_PageNumber_1()
        {
            var currentPage = 1;
            var pageInfo = new PageInfo(currentPage, TOTAL_ITEMS, PAGE_SIZE);
            var pagination = new Pagination(pageInfo, null, BUTTONS_ON_PAGE);
            var expectedStart = 1;
            var expectedEnd = 7;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }

        [TestMethod]
        public void PaginationInterval_PageNumber_2()
        {
            var currentPage = 2;
            var pageInfo = new PageInfo(currentPage, TOTAL_ITEMS, PAGE_SIZE);
            var pagination = new Pagination(pageInfo, null, BUTTONS_ON_PAGE);
            var expectedStart = 1;
            var expectedEnd = 7;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }

        [TestMethod]
        public void PaginationInterval_PageNumber_3()
        {
            var currentPage = 3;
            var pageInfo = new PageInfo(currentPage, TOTAL_ITEMS, PAGE_SIZE);
            var pagination = new Pagination(pageInfo, null, BUTTONS_ON_PAGE);
            var expectedStart = 1;
            var expectedEnd = 7;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }

        [TestMethod]
        public void PaginationInterval_PageNumber_4()
        {
            var currentPage = 4;
            var pageInfo = new PageInfo(currentPage, TOTAL_ITEMS, PAGE_SIZE);
            var pagination = new Pagination(pageInfo, null, BUTTONS_ON_PAGE);
            var expectedStart = 1;
            var expectedEnd = 7;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }

        [TestMethod]
        public void PaginationInterval_PageNumber_5()
        {
            var currentPage = 5;
            var pageInfo = new PageInfo(currentPage, TOTAL_ITEMS, PAGE_SIZE);
            var pagination = new Pagination(pageInfo, null, BUTTONS_ON_PAGE);
            var expectedStart = 2;
            var expectedEnd = 8;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }

        [TestMethod]
        public void PaginationInterval_PageNumber_6()
        {
            var currentPage = 6;
            var pageInfo = new PageInfo(currentPage, TOTAL_ITEMS, PAGE_SIZE);
            var pagination = new Pagination(pageInfo, null, BUTTONS_ON_PAGE);
            var expectedStart = 3;
            var expectedEnd = 9;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }

        [TestMethod]
        public void PaginationInterval_PageNumber_7()
        {
            var currentPage = 7;
            var pageInfo = new PageInfo(currentPage, TOTAL_ITEMS, PAGE_SIZE);
            var pagination = new Pagination(pageInfo, null, BUTTONS_ON_PAGE);
            var expectedStart = 3;
            var expectedEnd = 9;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }

        [TestMethod]
        public void PaginationInterval_PageNumber_8()
        {
            var currentPage = 8;
            var pageInfo = new PageInfo(currentPage, TOTAL_ITEMS, PAGE_SIZE);
            var pagination = new Pagination(pageInfo, null, BUTTONS_ON_PAGE);
            var expectedStart = 3;
            var expectedEnd = 9;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }

        [TestMethod]
        public void PaginationInterval_PageNumber_9()
        {
            var currentPage = 9;
            var pageInfo = new PageInfo(currentPage, TOTAL_ITEMS, PAGE_SIZE);
            var pagination = new Pagination(pageInfo, null, BUTTONS_ON_PAGE);
            var expectedStart = 3;
            var expectedEnd = 9;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }
    }
}
