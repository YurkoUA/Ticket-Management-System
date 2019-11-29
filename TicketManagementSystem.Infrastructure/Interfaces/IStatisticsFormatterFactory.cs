using TicketManagementSystem.ViewModels.Statistics.Enums;

namespace TicketManagementSystem.Infrastructure.Interfaces
{
    public interface IStatisticsFormatterFactory
    {
        IStatisticsFormatter GetFormatter(Chart chart);
    }
}
