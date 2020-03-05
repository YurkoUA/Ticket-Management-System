using System;
using System.Threading.Tasks;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;

namespace TicketManagementSystem.Domain.Ticket.Commands
{
    public class RemoveTicketCH : ICommandHandlerAsync<RemoveTicketCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveTicketCH(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(RemoveTicketCommand command)
        {
            var ticket = await _unitOfWork.Get<Data.Entities.Ticket>().FindAsync(command.TicketId);

            if (ticket == null)
            {
                throw new Exception();
            }

            await _unitOfWork.Get<Data.Entities.Ticket>().RemoveAsync(ticket);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
