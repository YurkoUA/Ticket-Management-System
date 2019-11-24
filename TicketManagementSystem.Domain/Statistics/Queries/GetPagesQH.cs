using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagementSystem.Data.Entities;
using TicketManagementSystem.Domain.Cqrs;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Util;
using TicketManagementSystem.ViewModels.Statistics;

namespace TicketManagementSystem.Domain.Statistics.Queries
{
    public class GetPagesQH : IQueryHandlerAsync<EmptyQuery<IEnumerable<PageVM>>, IEnumerable<PageVM>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEntityService entityService;

        public GetPagesQH(IUnitOfWork unitOfWork, IEntityService entityService)
        {
            this.unitOfWork = unitOfWork;
            this.entityService = entityService;
        }

        public async Task<IEnumerable<PageVM>> GetAsync(EmptyQuery<IEnumerable<PageVM>> query)
        {
            var pages = (await unitOfWork.Get<StatisticsPage>().FindAllAsync())
                .OrderBy(p => p.SortOrder);

            var pagesVM = entityService.ConvertCollection<StatisticsPage, PageVM>(pages);
            return pagesVM;
        }
    }
}
