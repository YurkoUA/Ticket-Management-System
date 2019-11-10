using System;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.ViewModels.Statistics;

namespace TicketManagementSystem.Domain.Statistics.Queries
{
    public class GetChartDataQuery : IQuery<ChartDataVM>
    {
        public int ChartId { get; set; }

        // Period.
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Moment.
        public DateTime Date { get; set; }
    }
}
