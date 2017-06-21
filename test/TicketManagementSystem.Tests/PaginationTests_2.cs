using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagementSystem.Web;
using TicketManagementSystem.Web.Data;

namespace TicketManagementSystem.Tests
{
    [TestClass]
    public class PaginationTests_2
    {
        [TestMethod]
        public void PaginationInterval_Items_10_Size_3_Buttons_3_PAGE_1()
        {
            var currentPage = 1;
            var pageInfo = new PageInfo(currentPage, 10, 3);
            var pagination = new Pagination(pageInfo, null, 3);
            var expectedStart = 1;
            var expectedEnd = 3;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }

        [TestMethod]
        public void PaginationInterval_Items_30_Size_10_Buttons_100_PAGE_2()
        {
            var currentPage = 2;
            var pageInfo = new PageInfo(currentPage, 30, 10);
            var pagination = new Pagination(pageInfo, null, 100);
            var expectedStart = 1;
            var expectedEnd = 3;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }

        [TestMethod]
        public void PaginationInterval_Items_100_Size_10_Buttons_20_PAGE_9()
        {
            var currentPage = 9;
            var pageInfo = new PageInfo(currentPage, 100, 10);
            var pagination = new Pagination(pageInfo, null, 20);
            var expectedStart = 1;
            var expectedEnd = 10;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }

        [TestMethod]
        public void PaginationInterval_Items_100_Size_10_Buttons_20_PAGE_1()
        {
            var currentPage = 1;
            var pageInfo = new PageInfo(currentPage, 100, 10);
            var pagination = new Pagination(pageInfo, null, 20);
            var expectedStart = 1;
            var expectedEnd = 10;

            var interval = pagination.GetPaginationInterval();

            Assert.AreEqual(expectedStart, interval.Item1);
            Assert.AreEqual(expectedEnd, interval.Item2);
        }
    }
}
