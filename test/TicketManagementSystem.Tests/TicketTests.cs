using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagementSystem.Business.Extensions;

namespace TicketManagementSystem.Tests
{
    [TestClass]
    public class TicketTests
    {
        [TestMethod]
        public void IsHappy_006600()
        {
            Assert.IsTrue("006600".IsHappy());
        }

        [TestMethod]
        public void IsHappy_100100()
        {
            Assert.IsTrue("100100".IsHappy());
        }

        [TestMethod]
        public void IsHappy_111111()
        {
            Assert.IsTrue("111111".IsHappy());
        }

        [TestMethod]
        public void IsHappy_317623()
        {
            Assert.IsTrue("317623".IsHappy());
        }

        [TestMethod]
        public void IsNotHappy_000000()
        {
            Assert.IsFalse("000000".IsHappy());
        }

        [TestMethod]
        public void IsNotHappy_219623()
        {
            Assert.IsFalse("219623".IsHappy());
        }

        [TestMethod]
        public void IsNotHappy_001002()
        {
            Assert.IsFalse("001002".IsHappy());
        }
    }
}
