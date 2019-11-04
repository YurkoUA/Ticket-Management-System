using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.ValidationChain;
using TicketManagementSystem.Infrastructure.ChainOfResponsibility;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Util;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Package.Commands
{
    public class EditPackageCH : ICommandHandlerAsync<EditPackageCommand, CommandResultVM<object>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEntityService entityService;

        public EditPackageCH(IUnitOfWork unitOfWork, IEntityService entityService)
        {
            this.unitOfWork = unitOfWork;
            this.entityService = entityService;
        }

        public async Task<CommandResultVM<object>> ExecuteAsync(EditPackageCommand command)
        {
            var result = new CommandResultDTO<object>();

            var chainBuilder = new ChainBuilder<IList<CommandMessageDTO>>();
            chainBuilder
                .ConstructChain(new ColorExistsValidator(unitOfWork, command.ColorId))
                .ConstructChain(new SerialExistsValidator(unitOfWork, command.SerialId))
                .ConstructChain(new NominalExistsValidator(unitOfWork, command.NominalId))
                .ConstructChain(new PackageFirstDigitValidator(unitOfWork, command.Id, command.FirstDigit))
                .ConstructChain(new PackagePropertiesChangedValidator(unitOfWork, command));

            chainBuilder.Head.HandleRequest(result.Errors);

            if (result.IsSuccess)
            {
                var package = await unitOfWork.Get<Data.Entities.Package>().FindAsync(command.Id);
                entityService.Assign(command, package);
                await unitOfWork.SaveChangesAsync();
            }

            return entityService.Convert<CommandResultDTO<object>, CommandResultVM<object>>(result);
        }
    }
}
