using Ninject;
using TicketManagementSystem.Infrastructure.Interfaces;
using TicketManagementSystem.ViewModels.Statistics.Enums;

namespace TicketManagementSystem.Domain.Statistics.Formatters
{
    public class StatisticsFormatterFactory : IStatisticsFormatterFactory
    {
        private readonly IKernel kernel;

        public StatisticsFormatterFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public IStatisticsFormatter GetFormatter(Chart chart)
        {
            return kernel.TryGet<IStatisticsFormatter>(chart.ToString());
        }
    }
}
