using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketManagementSystem.Data.Entities;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.ValidationChain;
using TicketManagementSystem.Tests.Helpers;

namespace TicketManagementSystem.Tests.Domain.ValidationChain
{
    [TestClass]
    public class PackageFirstDigitValidatorTest : BaseTest
    {
        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestMethod]
        public void HandleRequest_DoesNot_AddError_When_FirstDigit_null()
        {
            int? firstDigit = null;
            var validator = GetValidator(1, firstDigit);
            var errors = new List<CommandMessageDTO>();

            validator.HandleRequest(errors);

            CollectionAssertHelper.IsEmpty(errors);
        }

        [TestMethod]
        public void HandleRequest_DoesNot_AddError_When_Package_IsEmpty()
        {
            _ticketRepoMock.Setup(r => r.Any(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns(false);

            var validator = GetValidator(1, 1);
            var errors = new List<CommandMessageDTO>();

            validator.HandleRequest(errors);

            CollectionAssertHelper.IsEmpty(errors);
        }

        [TestMethod]
        public void HandleRequest_DoesNot_AddError_When_AllTickets_FromPackage_Have_Given_FirstDigit()
        {
            int? firstDigit = 2;
            var packageId = 1;

            _ticketRepoMock.Setup(r => r.Any(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns(true);
            _ticketRepoMock.Setup(r => r.FindAll(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns(new List<Ticket> 
            { 
                new Ticket { PackageId = 33 },
                new Ticket { PackageId = 33 },
                new Ticket { PackageId = null }
            }.AsQueryable());

            var validator = GetValidator(packageId, firstDigit);
            var errors = new List<CommandMessageDTO>();

            validator.HandleRequest(errors);

            CollectionAssertHelper.IsEmpty(errors);
        }

        [TestMethod]
        public void HandleRequest_Adds_AnError_When_SomeTickets_FromPackage_Have_Another_FirstDigit()
        {
            int? firstDigit = 2;
            var packageId = 1;

            _ticketRepoMock.Setup(r => r.Any(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns(true);
            _ticketRepoMock.Setup(r => r.FindAll(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns(new List<Ticket>
            {
                new Ticket { PackageId = packageId },
                new Ticket { PackageId = packageId },

                new Ticket { PackageId = 33 },
                new Ticket { PackageId = 33 },
                new Ticket { PackageId = null }
            }.AsQueryable());

            var validator = GetValidator(packageId, firstDigit);
            var errors = new List<CommandMessageDTO>();

            validator.HandleRequest(errors);

            CollectionAssertHelper.Count(errors, 1);
            Assert.AreEqual(ValidationMessage.PACKAGE_FIRST_DIGIT_CANNOT_BE_SET, errors.ElementAt(0).ResourceName);
            Assert.IsNull(errors.ElementAt(0).Arguments);
        }

        private PackageFirstDigitValidator GetValidator(int packageId, int? firstDigit)
        {
            return new PackageFirstDigitValidator(_unitOfWorkMock.Object, packageId, firstDigit);
        }
    }
}
