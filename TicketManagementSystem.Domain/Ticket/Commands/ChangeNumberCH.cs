using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.DTO.Ticket;
using TicketManagementSystem.Domain.ValidationChain;
using TicketManagementSystem.Infrastructure.ChainOfResponsibility;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Util;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Ticket.Commands
{
    public class ChangeNumberCH : ICommandHandlerAsync<ChangeNumberCommand, CommandResultVM<object>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEntityService entityService;

        public ChangeNumberCH(IUnitOfWork unitOfWork, IEntityService entityService)
        {
            this.unitOfWork = unitOfWork;
            this.entityService = entityService;
        }

        public async Task<CommandResultVM<object>> ExecuteAsync(ChangeNumberCommand command)
        {
            var ticketRepo = unitOfWork.Get<Data.Entities.Ticket>();
            var origin = await ticketRepo.FindAsync(command.Id);

            if (origin == null)
            {
                throw new Exception();
            }

            var result = new CommandResultDTO<object>();
            var dto = entityService.Convert<Data.Entities.Ticket, TicketDTO>(origin);
            dto.Number = command.Number;

            var chainBuilder = new ChainBuilder<IList<CommandMessageDTO>>();
            chainBuilder
                .ConstructChain(new TicketNumberValidator(unitOfWork, dto))
                .ConstructChain(new PackageSupportsTicketValidator(unitOfWork, dto));

            chainBuilder.Head.HandleRequest(result.Errors);

            if (result.IsSuccess)
            {
                origin.Number = command.Number;
                await ticketRepo.UpdateAsync(origin);
                await unitOfWork.SaveChangesAsync();
            }

            return entityService.Convert<CommandResultDTO<object>, CommandResultVM<object>>(result);
        }
    }
}
