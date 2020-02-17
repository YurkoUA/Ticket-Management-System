﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.DTO.Ticket;
using TicketManagementSystem.Domain.ValidationChain;
using TicketManagementSystem.Domain.ValidationChain.Models;
using TicketManagementSystem.Infrastructure.ChainOfResponsibility;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Util;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Ticket.Commands
{
    public class CreateTicketCH : ICommandHandlerAsync<CreateTicketCommand, CommandResultVM<IdentifierVM>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEntityService entityService;

        public CreateTicketCH(IUnitOfWork unitOfWork, IEntityService entityService)
        {
            this.unitOfWork = unitOfWork;
            this.entityService = entityService;
        }

        public async Task<CommandResultVM<IdentifierVM>> ExecuteAsync(CreateTicketCommand command)
        {
            var result = new CommandResultDTO<IdentifierVM>();
            var dto = entityService.Convert<CreateTicketCommand, TicketDTO>(command);

            var chainBuilder = new ChainBuilder<IList<CommandMessageDTO>>();
            chainBuilder
                .ConstructChain(new ColorExistsValidator(unitOfWork, command.ColorId))
                .ConstructChain(new SerialExistsValidator(unitOfWork, command.SerialId))
                .ConstructChain(new NominalExistsValidator(unitOfWork, command.NominalId));

            if (command.PackageId.HasValue)
            {
                chainBuilder
                    .ConstructChain(new PackageExistsValidator(unitOfWork, command.PackageId, false))
                    .ConstructChain(new PackageStateValidator(unitOfWork, command.PackageId.Value))
                    .ConstructChain(new TicketNumberValidator(unitOfWork, dto))
                    .ConstructChain(new PackageSupportsTicketValidator(unitOfWork, dto));
            }

            chainBuilder.Head.HandleRequest(result.Errors);

            if (result.IsSuccess)
            {
                var ticket = entityService.Convert<CreateTicketCommand, Data.Entities.Ticket>(command);
                ticket = await unitOfWork.Get<Data.Entities.Ticket>().CreateAsync(ticket);
                await unitOfWork.SaveChangesAsync();

                result.Model = new IdentifierVM { Id = ticket.Id };
            }

            return entityService.Convert<CommandResultDTO<IdentifierVM>, CommandResultVM<IdentifierVM>>(result);
        }
    }
}