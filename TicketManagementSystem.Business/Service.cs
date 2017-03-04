using TicketManagementSystem.Data.Interfaces;

namespace TicketManagementSystem.Business
{
    public abstract class Service<T> where T : class
    {
        protected UnitOfWork _uow;
        protected IRepository<T> _repo;

        public Service()
        {
            _uow = UnitOfWork.GetInstance();
        }

        public UnitOfWork UoW => _uow;
        public IRepository<T> Repository => _repo;
    }
}
