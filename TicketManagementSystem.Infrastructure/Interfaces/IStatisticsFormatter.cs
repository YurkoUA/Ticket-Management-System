using System.Collections.Generic;
using TicketManagementSystem.ViewModels.Statistics;

namespace TicketManagementSystem.Infrastructure.Interfaces
{
    public interface IStatisticsFormatter
    {
        void Format(ChartDataVM chartData);
    }
}
