using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Tests
{
    [TestClass]
    public class TicketTests
    {
        [TestMethod]
        public void IsHappy_000000()
        {
            var ticket = new Ticket { Number = "000000" };
            Assert.IsTrue(ticket.IsHappy());
        }

        [TestMethod]
        public void IsHappy_006600()
        {
            var ticket = new Ticket { Number = "006600" };
            Assert.IsTrue(ticket.IsHappy());
        }

        [TestMethod]
        public void IsHappy_100100()
        {
            var ticket = new Ticket { Number = "100100" };
            Assert.IsTrue(ticket.IsHappy());
        }

        [TestMethod]
        public void IsHappy_111111()
        {
            var ticket = new Ticket { Number = "111111" };
            Assert.IsTrue(ticket.IsHappy());
        }

        [TestMethod]
        public void IsHappy_317623()
        {
            var ticket = new Ticket { Number = "317623" };
            Assert.IsTrue(ticket.IsHappy());
        }

        [TestMethod]
        public void IsNotHappy_219623()
        {
            var ticket = new Ticket { Number = "219623" };
            Assert.IsFalse(ticket.IsHappy());
        }

        [TestMethod]
        public void IsNotHappy_001002()
        {
            var ticket = new Ticket { Number = "001002" };
            Assert.IsFalse(ticket.IsHappy());
        }
    }
}
