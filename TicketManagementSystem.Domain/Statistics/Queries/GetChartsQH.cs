using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagementSystem.Data.Entities;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Util;
using TicketManagementSystem.ViewModels.Statistics;

namespace TicketManagementSystem.Domain.Statistics.Queries
{
    public class GetChartsQH : IQueryHandlerAsync<GetChartsQuery, IEnumerable<ChartInfoVM>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEntityService entityService;

        public GetChartsQH(IUnitOfWork unitOfWork, IEntityService entityService)
        {
            this.unitOfWork = unitOfWork;
            this.entityService = entityService;
        }

        public async Task<IEnumerable<ChartInfoVM>> GetAsync(GetChartsQuery query)
        {
            var charts = (await unitOfWork.Get<StatisticsChart>()
                    .FindAllAsync(c => !query.PageId.HasValue || c.PageId == query.PageId))
                .AsEnumerable();

            var chartsVm = entityService.ConvertCollection<StatisticsChart, ChartInfoVM>(charts);
            return chartsVm;
        }
    }
}
