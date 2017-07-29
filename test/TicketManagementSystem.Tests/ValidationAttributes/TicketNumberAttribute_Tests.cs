using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagementSystem.Business.Attributes;

namespace TicketManagementSystem.Tests.ValidationAttributes
{
    [TestClass]
    public class TicketNumberAttribute_Tests
    {
        private TicketNumberAttribute _attribute;

        [TestInitialize]
        public void Initialize()
        {
            _attribute = new TicketNumberAttribute();
        }

        [TestMethod]
        public void Number_317623_Must_Return_True()
        {
            var number = "317623";

            Assert.IsTrue(_attribute.IsValid(number));
        }

        [TestMethod]
        public void Number_219624_Must_Return_True()
        {
            var number = "219623";

            Assert.IsTrue(_attribute.IsValid(number));
        }

        [TestMethod]
        public void Number_317623132_Must_Return_False()
        {
            var number = "317623132";

            Assert.IsFalse(_attribute.IsValid(number));
        }

        [TestMethod]
        public void Number_123abc_Must_Return_False()
        {
            var number = "123abc";

            Assert.IsFalse(_attribute.IsValid(number));
        }

        [TestMethod]
        public void Number_000000_Must_Return_False()
        {
            var number = "000000";

            Assert.IsFalse(_attribute.IsValid(number));
        }

        [TestMethod]
        public void Number_123_Must_Return_False()
        {
            var number = "123";

            Assert.IsFalse(_attribute.IsValid(number));
        }
    }
}
