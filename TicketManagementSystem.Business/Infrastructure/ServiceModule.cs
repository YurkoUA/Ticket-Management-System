using Ninject.Modules;
using TicketManagementSystem.Data.EF;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string _connectionString;

        public ServiceModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(_connectionString);
        }
    }
}
