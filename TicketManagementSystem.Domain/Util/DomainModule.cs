using Ninject.Modules;
using TicketManagementSystem.Data;
using TicketManagementSystem.Domain.Color.Queries;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Domain.Processors;
using TicketManagementSystem.ViewModels.Color;

namespace TicketManagementSystem.Domain.Util
{
    public class DomainModule : NinjectModule
    {
        private string _connectionString;

        public DomainModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override void Load()
        {
            Bind<AppDbContext>().To<AppDbContext>().WithConstructorArgument(_connectionString);
            Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            Bind<IUnitOfWork>().To<UnitOfWork>();

            Bind<IQueryProcessorAsync>().To<QueryProcessorAsync>();
            Bind<ICommandProcessorAsync>().To<CommandProcessorAsync>();

            Bind<IQueryHandlerAsync<GetColorQuery, ColorVM>>().To<GetColorQueryHandlerAsync>();
        }
    }
}
