using System.Collections.Generic;
using Ninject.Modules;
using TicketManagementSystem.Data;
using TicketManagementSystem.Domain.Color.Queries;
using TicketManagementSystem.Domain.Cqrs;
using TicketManagementSystem.Domain.Nominal.Queries;
using TicketManagementSystem.Domain.Package.Commands;
using TicketManagementSystem.Domain.Statistics.Formatters;
using TicketManagementSystem.Domain.Statistics.Queries;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Domain.Processors;
using TicketManagementSystem.Infrastructure.Interfaces;
using TicketManagementSystem.ViewModels.Color;
using TicketManagementSystem.ViewModels.Common;
using TicketManagementSystem.ViewModels.Nominal;
using TicketManagementSystem.ViewModels.Statistics;
using TicketManagementSystem.ViewModels.Statistics.Enums;

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
            Bind<IParameterFactory>().To<SqlParameterFactory>();

            Bind<IStatisticsFormatterFactory>().To<StatisticsFormatterFactory>();

            Bind<IStatisticsFormatter>().To<TicketMonthSummaryFormatter>().Named(Chart.TicketMonthSummary.ToString());

            Bind<IQueryProcessorAsync>().To<QueryProcessorAsync>();
            Bind<ICommandProcessorAsync>().To<CommandProcessorAsync>();

            Bind<IQueryHandlerAsync<GetColorQuery, ColorVM>>().To<GetColorQH>();
            Bind<IQueryHandlerAsync<EmptyQuery<IEnumerable<NominalVM>>, IEnumerable<NominalVM>>>().To<GetNominalsQH>();

            Bind<ICommandHandlerAsync<CreatePackageCommand, CommandResultVM<IdentifierVM>>>().To<CreatePackageCH>();
            Bind<ICommandHandlerAsync<CreateSpecialPackageCommand, CommandResultVM<IdentifierVM>>>().To<CreateSpecialPackageCH>();

            Bind<ICommandHandlerAsync<EditPackageCommand, CommandResultVM<object>>>().To<EditPackageCH>();
            Bind<ICommandHandlerAsync<EditSpecialPackageCommand, CommandResultVM<object>>>().To<EditSpecialPackageCH>();

            Bind<IQueryHandlerAsync<GetChartsQuery, IEnumerable<ChartInfoVM>>>().To<GetChartsQH>();
            Bind<IQueryHandlerAsync<GetChartDataQuery, ChartDataVM>>().To<GetChartDataQH>();
            Bind<IQueryHandlerAsync<EmptyQuery<IEnumerable<PageVM>>, IEnumerable<PageVM>>>().To<GetPagesQH>();
        }
    }
}
