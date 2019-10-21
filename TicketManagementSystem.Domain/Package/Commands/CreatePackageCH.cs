using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Constants;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.ValidationChain;
using TicketManagementSystem.Infrastructure.ChainOfResponsibility;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Util;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Package.Commands
{
    public class CreatePackageCH : ICommandHandlerAsync<CreatePackageCommand, CommandResultVM<IdentifierVM>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEntityService entityService;

        public CreatePackageCH(IUnitOfWork unitOfWork, IEntityService entityService)
        {
            this.unitOfWork = unitOfWork;
            this.entityService = entityService;
        }

        public async Task<CommandResultVM<IdentifierVM>> ExecuteAsync(CreatePackageCommand command)
        {
            var result = new CommandResultDTO<IdentifierVM>();

            var chainBuilder = new ChainBuilder<IList<CommandMessageDTO>>();
            chainBuilder
                .ConstructChain(new ColorExistsValidator(unitOfWork, command.ColorId))
                .ConstructChain(new SerialExistsValidator(unitOfWork, command.SerialId))
                .ConstructChain(new NominalExistsValidator(unitOfWork, command.NominalId));

            chainBuilder.Head.HandleRequest(result.Errors);

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
