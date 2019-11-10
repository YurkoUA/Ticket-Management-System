using System.Collections.Generic;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.ViewModels.Statistics;

namespace TicketManagementSystem.Domain.Statistics.Queries
{
    public class GetChartsQuery : IQuery<IEnumerable<ChartInfoVM>>
    {
        public int? PageId { get; set; }
    }
}
