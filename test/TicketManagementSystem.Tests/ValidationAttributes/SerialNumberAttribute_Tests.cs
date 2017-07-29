using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagementSystem.Business.Attributes;

namespace TicketManagementSystem.Tests.ValidationAttributes
{
    [TestClass]
    public class SerialNumberAttribute_Tests
    {
        private SerialNumberAttribute _attribute;

        [TestInitialize]
        public void Initialize()
        {
            _attribute = new SerialNumberAttribute();
        }

        [TestMethod]
        public void SerialNumber_50_Must_Return_True()
        {
            var serialNumber = "50";

            Assert.IsTrue(_attribute.IsValid(serialNumber));
        }

        [TestMethod]
        public void SerialNumber_01_Must_Return_True()
        {
            var serialNumber = "01";

            Assert.IsTrue(_attribute.IsValid(serialNumber));
        }

        [TestMethod]
        public void SerialNumber_33_Must_Return_True()
        {
            var serialNumber = "33";

            Assert.IsTrue(_attribute.IsValid(serialNumber));
        }

        [TestMethod]
        public void SerialNumber_00_Must_Return_False()
        {
            var serialNumber = "00";

            Assert.IsFalse(_attribute.IsValid(serialNumber));
        }

        [TestMethod]
        public void SerialNumber_51_Must_Return_False()
        {
            var serialNumber = "51";

            Assert.IsFalse(_attribute.IsValid(serialNumber));
        }

        [TestMethod]
        public void SerialNumber_77_Must_Return_False()
        {
            var serialNumber = "77";

            Assert.IsFalse(_attribute.IsValid(serialNumber));
        }

        [TestMethod]
        public void SerialNumber_100_Must_Return_False()
        {
            var serialNumber = "100";

            Assert.IsFalse(_attribute.IsValid(serialNumber));
        }

        [TestMethod]
        public void SerialNumber_neg1_Must_Return_False()
        {
            var serialNumber = "-1";

            Assert.IsFalse(_attribute.IsValid(serialNumber));
        }
    }
}
