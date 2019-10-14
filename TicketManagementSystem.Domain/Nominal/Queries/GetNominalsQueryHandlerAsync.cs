using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Cqrs;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Util;
using TicketManagementSystem.ViewModels.Nominal;

namespace TicketManagementSystem.Domain.Nominal.Queries
{
    public class GetNominalsQueryHandlerAsync : IQueryHandlerAsync<EmptyQuery<IEnumerable<NominalVM>>, IEnumerable<NominalVM>>
    {
        private readonly IRepository<Data.Entities.Nominal> repo;
        private readonly IEntityService entityService;

        public GetNominalsQueryHandlerAsync(IRepository<Data.Entities.Nominal> repo, IEntityService entityService)
        {
            this.repo = repo;
            this.entityService = entityService;
        }

        public async Task<IEnumerable<NominalVM>> GetAsync(EmptyQuery<IEnumerable<NominalVM>> query)
        {
            var nominals = (await repo.FindAllAsync()).ToList();
            return entityService.ConvertCollection<Data.Entities.Nominal, NominalVM>(nominals);
        }
    }
}
