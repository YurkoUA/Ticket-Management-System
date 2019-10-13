using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Exceptions;
using TicketManagementSystem.ViewModels.Color;

namespace TicketManagementSystem.Domain.Color.Queries
{
    public class GetColorQueryHandlerAsync : IQueryHandlerAsync<GetColorQuery, ColorVM>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetColorQueryHandlerAsync(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ColorVM> GetAsync(GetColorQuery query)
        {
            var repo = unitOfWork.Get<Data.Entities.Color>();
            var color = (await repo.FindAllIncludingAsync(c => c.Id == query.ColorId, c => c.Tickets, c => c.Packages))
                .Select(c => new ColorVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    PackagesCount = c.Packages.Count,
                    TicketsCount = c.Tickets.Count
                }).FirstOrDefault();

            if (color == null)
            {
                throw new NotFoundException();
            }

            color.CanBeDeleted = color.PackagesCount == 0 && color.TicketsCount == 0;
            return color;
        }
    }
}
