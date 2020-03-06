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
    public class MoveTicketCH : ICommandHandlerAsync<MoveTicketCommand, CommandResultVM<object>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEntityService entityService;

        public MoveTicketCH(IUnitOfWork unitOfWork, IEntityService entityService)
        {
            this.unitOfWork = unitOfWork;
            this.entityService = entityService;
        }

        public async Task<CommandResultVM<object>> ExecuteAsync(MoveTicketCommand command)
        {
            var ticketRepo = unitOfWork.Get<Data.Entities.Ticket>();
            var origin = await ticketRepo.FindAsync(command.TicketId);

            if (origin == null)
            {
                throw new Exception();
            }

            var result = new CommandResultDTO<object>();
            var dto = entityService.Convert<Data.Entities.Ticket, TicketDTO>(origin);
            dto.PackageId = command.PackageId;

            if (command.PackageId.HasValue)
            {
                var chainBuilder = new ChainBuilder<IList<CommandMessageDTO>>();
                chainBuilder
                    .ConstructChain(new PackageExistsValidator(unitOfWork, dto.PackageId, false))
                    .ConstructChain(new PackageSupportsTicketValidator(unitOfWork, dto));
            }

            if (!command.PackageId.HasValue || result.IsSuccess)
            {
                origin.PackageId = command.PackageId;
                await ticketRepo.UpdateAsync(origin);
                await unitOfWork.SaveChangesAsync();
            }

            return entityService.Convert<CommandResultDTO<object>, CommandResultVM<object>>(result);
        }
    }
}
