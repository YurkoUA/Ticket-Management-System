using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TicketManagementSystem.Data.Entities;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.ViewModels.Statistics;
using TicketManagementSystem.ViewModels.Statistics.Enums;

namespace TicketManagementSystem.Domain.Statistics.Queries
{
    public class GetChartDataQH : IQueryHandlerAsync<GetChartDataQuery, ChartDataVM>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IParameterFactory parameterFactory;

        public GetChartDataQH(IUnitOfWork unitOfWork, IParameterFactory parameterFactory)
        {
            this.unitOfWork = unitOfWork;
            this.parameterFactory = parameterFactory;
        }

        public async Task<ChartDataVM> GetAsync(GetChartDataQuery query)
        {
            var chart = await unitOfWork.Get<StatisticsChart>().FindAsync(query.ChartId);

            if (chart == null)
            {
                // TODO: Customize type and move message to resources.
                throw new Exception("The chart isn't found.");
            }

            var parameters = GetParameters(query, (ChartComputingStrategy)chart.ComputingStrategyId);
            var chartData = await unitOfWork.ExecuteProcedureAsync<ChartDataItemVM>(chart.SPName, parameters);
            return new ChartDataVM
            {
                Data = chartData
            };
        }

        internal IEnumerable<IDbDataParameter> GetParameters(GetChartDataQuery query, ChartComputingStrategy computingStrategy)
        {
            if (computingStrategy == ChartComputingStrategy.Moment)
            {
                return parameterFactory.CreateParameterList(query, q => q.Date);
            }
            else
            {
                return parameterFactory.CreateParameterList(query, q => q.StartDate, q => q.EndDate);
            }
        }
    }
}
