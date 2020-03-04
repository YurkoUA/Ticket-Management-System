using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagementSystem.Data.Entities;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.DTO.Ticket;
using TicketManagementSystem.Domain.ValidationChain;
using TicketManagementSystem.Tests.Helpers;

namespace TicketManagementSystem.Tests.Domain.ValidationChain
{
    [TestClass]
    public class TicketNumberValidatorTest : BaseTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            MockTickets();
        }

        [DataTestMethod]
        [DataRow(1), DataRow(null)]
        public void DoesNotAddError_When_Ticket_DoesNotExist_in_Repo(int? id)
        {
            var ticket = new TicketDTO
            {
                Id = id,
                Number = "242343",
                NominalId = 1,
                ColorId = 2,
                SerialId = 3,
                SerialNumber = "99"
            };

            var validator = GetValidator(ticket);
            var errors = new List<CommandMessageDTO>();

            validator.HandleRequest(errors);

            CollectionAssertHelper.IsEmpty(errors);
        }

        [DataTestMethod]
        [DataRow("111111", 1, 1, 1, "22")]
        [DataRow("111111", 1, 1, 2, "11")]
        [DataRow("111111", 1, 2, 1, "11")]
        [DataRow("111111", 2, 1, 1, "11")]
        [DataRow("222222", 1, 1, 1, "11")]
        public void DoesNotAddError_When_FullMatch_NotFound_and_Id_null(string number, int nominal, int color, int serial, string serialNumber)
        {
            var ticket = new TicketDTO
            {
                Number = number,
                NominalId = nominal,
                ColorId = color,
                SerialId = serial,
                SerialNumber = serialNumber
            };

            var validator = GetValidator(ticket);
            var errors = new List<CommandMessageDTO>();

            validator.HandleRequest(errors);

            CollectionAssertHelper.IsEmpty(errors);
        }

        [DataTestMethod]
        [DataRow(null), DataRow(111)]
        public void AddsAnError_When_FullMatch_Found_and_Id_null_or_different(int? id)
        {
            var ticket = new TicketDTO
            {
                Id = id,
                Number = "111111",
                NominalId = 1,
                ColorId = 1,
                SerialId = 1,
                SerialNumber = "11"
            };

            var validator = GetValidator(ticket);
            var errors = new List<CommandMessageDTO>();

            validator.HandleRequest(errors);

            CollectionAssertHelper.Count(errors, 1);
            Assert.AreEqual(ValidationMessage.TICKET_ALREADY_EXISTS, errors.ElementAt(0).ResourceName);
            CollectionAssertHelper.Count(errors.ElementAt(0).Arguments, 1);
            Assert.AreEqual(ticket.Number, errors.ElementAt(0).Arguments.ElementAt(0));
        }

        [TestMethod]
        public void DoesNotAddError_When_FullMatch_Found_but_Id_the_same()
        {
            var ticket = new TicketDTO
            {
                Id = 1,
                Number = "111111",
                NominalId = 1,
                ColorId = 1,
                SerialId = 1,
                SerialNumber = "11"
            };

            var validator = GetValidator(ticket);
            var errors = new List<CommandMessageDTO>();

            validator.HandleRequest(errors);

            CollectionAssertHelper.IsEmpty(errors);
        }

        private TicketNumberValidator GetValidator(TicketDTO ticket)
        {
            return new TicketNumberValidator(_unitOfWorkMock.Object, ticket);
        }

        private void MockTickets()
        {
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Number = "111111", ColorId = 1, SerialId = 1, NominalId = 1, SerialNumber = "11" },
                new Ticket { Id = 2, Number = "222222", ColorId = 2, SerialId = 2, NominalId = 2, SerialNumber = "22" },
                new Ticket { Id = 3, Number = "333333", ColorId = 3, SerialId = 3, NominalId = 3, SerialNumber = "33" },
                new Ticket { Id = 4, Number = "444444", ColorId = 4, SerialId = 4, NominalId = 4, SerialNumber = "44" },
                new Ticket { Id = 5, Number = "555555", ColorId = 5, SerialId = 5, NominalId = 5, SerialNumber = "55" },
                new Ticket { Id = 6, Number = "666666", ColorId = 6, SerialId = 6, NominalId = 6, SerialNumber = "66" },
                new Ticket { Id = 7, Number = "777777", ColorId = 7, SerialId = 7, NominalId = 7, SerialNumber = "77" },
                new Ticket { Id = 8, Number = "888888", ColorId = 8, SerialId = 8, NominalId = 8, SerialNumber = "88" },
                new Ticket { Id = 9, Number = "999999", ColorId = 9, SerialId = 9, NominalId = 9, SerialNumber = "99" }
            };

            _ticketRepoMock.Setup(r => r.FindAll()).Returns(tickets.AsQueryable());
        }
    }
}
