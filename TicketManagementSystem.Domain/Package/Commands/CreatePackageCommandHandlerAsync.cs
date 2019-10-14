using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Util;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Package.Commands
{
    public class CreatePackageCommandHandlerAsync : ICommandHandlerAsync<CreatePackageCommand, CommandResultVM<IdentifierVM>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEntityService entityService;

        public CreatePackageCommandHandlerAsync(IUnitOfWork unitOfWork, IEntityService entityService)
        {
            this.unitOfWork = unitOfWork;
            this.entityService = entityService;
        }

        public async Task<CommandResultVM<IdentifierVM>> ExecuteAsync(CreatePackageCommand command)
        {
            var result = new CommandResultDTO<IdentifierVM>();

            if ((await unitOfWork.Get<Data.Entities.Color>().FindByIdAsync(command.ColorId)) == null)
            {
                result.IsSuccess = false;
                result.Errors.Add(new CommandMessageDTO
                {
                    ResourceName = ValidationMessage.COLOR_NOT_EXISTS,
                    Arguments = new object[] { command.ColorId }
                });
            }

            if ((await unitOfWork.Get<Data.Entities.Serial>().FindByIdAsync(command.SerialId)) == null)
            {
                result.IsSuccess = false;
                result.Errors.Add(new CommandMessageDTO
                {
                    ResourceName = ValidationMessage.SERIAL_NOT_EXISTS,
                    Arguments = new object[] { command.SerialId }
                });
            }

            if ((await unitOfWork.Get<Data.Entities.Nominal>().FindByIdAsync(command.NominalId)) == null)
            {
                result.IsSuccess = false;
                result.Errors.Add(new CommandMessageDTO
                {
                    ResourceName = ValidationMessage.NOMINAL_NOT_EXISTS,
                    Arguments = new object[] { command.NominalId }
                });
            }

            if (result.IsSuccess)
            {
                var package = entityService.Convert<CreatePackageCommand, Data.Entities.Package>(command);
                package = await unitOfWork.Get<Data.Entities.Package>().CreateAsync(package);
                await unitOfWork.SaveChangesAsync();
                result.Model = new IdentifierVM { Id = package.Id };
            }

            return entityService.Convert<CommandResultDTO<IdentifierVM>, CommandResultVM<IdentifierVM>>(result);
        }
    }
}
