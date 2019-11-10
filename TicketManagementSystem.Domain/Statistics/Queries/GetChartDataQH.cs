using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Data.Entities;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.ViewModels.Statistics;

namespace TicketManagementSystem.Domain.Statistics.Queries
{
    public class GetChartDataQH : IQueryHandlerAsync<GetChartDataQuery, ChartDataVM>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetChartDataQH(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ChartDataVM> GetAsync(GetChartDataQuery query)
        {
            var chart = unitOfWork.Get<StatisticsChart>().FindAsync(query.ChartId);

            if (chart == null)
            {
                // TODO: Customize type and move message to resources.
                throw new Exception("The chart isn't found.");
            }

            throw new NotImplementedException();
        }
    }
}
