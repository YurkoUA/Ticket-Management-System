using Moq;
using TicketManagementSystem.Data.Entities;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Tests
{
    public abstract class BaseTest
    {
        protected Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        protected Mock<IRepository<Color>> _colorRepoMock = new Mock<IRepository<Color>>();
        protected Mock<IRepository<Serial>> _serialRepoMock = new Mock<IRepository<Serial>>();
        protected Mock<IRepository<Nominal>> _nominalRepoMock = new Mock<IRepository<Nominal>>();
        protected Mock<IRepository<Package>> _packageRepoMock = new Mock<IRepository<Package>>();
        protected Mock<IRepository<Ticket>> _ticketRepoMock = new Mock<IRepository<Ticket>>();

        public BaseTest()
        {
            _unitOfWorkMock.Setup(m => m.Get<Color>()).Returns(_colorRepoMock.Object);
            _unitOfWorkMock.Setup(m => m.Get<Serial>()).Returns(_serialRepoMock.Object);
            _unitOfWorkMock.Setup(m => m.Get<Nominal>()).Returns(_nominalRepoMock.Object);
            _unitOfWorkMock.Setup(m => m.Get<Package>()).Returns(_packageRepoMock.Object);
            _unitOfWorkMock.Setup(m => m.Get<Ticket>()).Returns(_ticketRepoMock.Object);
        }
    }
}
