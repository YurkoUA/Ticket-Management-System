using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.ValidationChain;
using TicketManagementSystem.Domain.ValidationChain.Models;
using TicketManagementSystem.Infrastructure.ChainOfResponsibility;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.Infrastructure.Util;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Domain.Package.Commands
{
    public class EditSpecialPackageCH : ICommandHandlerAsync<EditSpecialPackageCommand, CommandResultVM<object>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEntityService entityService;

        public EditSpecialPackageCH(IUnitOfWork unitOfWork, IEntityService entityService)
        {
            this.unitOfWork = unitOfWork;
            this.entityService = entityService;
        }

        public async Task<CommandResultVM<object>> ExecuteAsync(EditSpecialPackageCommand command)
        {
            var result = new CommandResultDTO<object>();

            var chainBuilder = new ChainBuilder<IList<CommandMessageDTO>>();
            chainBuilder
                .ConstructChain(new PackageNameValidator(unitOfWork, command.Name, command.Id))
                .ConstructChain(new ColorExistsValidator(unitOfWork, command.ColorId, false))
                .ConstructChain(new SerialExistsValidator(unitOfWork, command.SerialId, false))
                .ConstructChain(new NominalExistsValidator(unitOfWork, command.NominalId, false))
                .ConstructChain(new PackagePropertiesChangedValidator(unitOfWork, new PackagePropertiesChangedValidatorContext
                {
                    PackageId = command.Id,
                    ColorId = command.ColorId,
                    SerialId = command.SerialId,
                    NominalId = command.NominalId
                }));

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
