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
    public class EditTicketCH : ICommandHandlerAsync<EditTicketCommand, CommandResultVM<object>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEntityService entityService;

        public EditTicketCH(IUnitOfWork unitOfWork, IEntityService entityService)
        {
            this.unitOfWork = unitOfWork;
            this.entityService = entityService;
        }

        public async Task<CommandResultVM<object>> ExecuteAsync(EditTicketCommand command)
        {
            var ticketRepo = unitOfWork.Get<Data.Entities.Ticket>();
            var original = await ticketRepo.FindAsync(command.Id);

            if (original == null)
            {
                throw new Exception();
            }

            var result = new CommandResultDTO<object>();
            var dto = entityService.Convert<EditTicketCommand, TicketDTO>(command);
            dto.Number = original.Number;
            dto.PackageId = original.PackageId;

            var chainBuilder = new ChainBuilder<IList<CommandMessageDTO>>();
            chainBuilder
                .ConstructChain(new ColorExistsValidator(unitOfWork, command.ColorId))
                .ConstructChain(new SerialExistsValidator(unitOfWork, command.SerialId))
                .ConstructChain(new NominalExistsValidator(unitOfWork, command.NominalId))
                .ConstructChain(new TicketNumberValidator(unitOfWork, dto));

            if (dto.PackageId.HasValue)
            {
                chainBuilder
                    .ConstructChain(new PackageExistsValidator(unitOfWork, dto.PackageId, false))
                    .ConstructChain(new PackageSupportsTicketValidator(unitOfWork, dto));
            }

            chainBuilder.Head.HandleRequest(result.Errors); 
            
            if (result.IsSuccess)
            {
                original.ColorId = command.ColorId;
                original.SerialId = command.SerialId;
                original.NominalId = command.NominalId;
                original.SerialNumber = command.SerialNumber;
                original.Note = command.Note;
                original.Date = command.Date;
            }

            await ticketRepo.UpdateAsync(original);
            await unitOfWork.SaveChangesAsync();

            return entityService.Convert<CommandResultDTO<object>, CommandResultVM<object>>(result);
        }
    }
}
