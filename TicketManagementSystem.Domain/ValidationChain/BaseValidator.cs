using System.Collections.Generic;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Infrastructure.ChainOfResponsibility;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain
{
    public abstract class BaseValidator : IChainElement<IList<CommandMessageDTO>>
    {
        protected IUnitOfWork unitOfWork;

        protected BaseValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IChainElement<IList<CommandMessageDTO>> Next { get; set; }

        public abstract void HandleRequest(IList<CommandMessageDTO> model);

        public void Continue(IList<CommandMessageDTO> model)
        {
            Next?.HandleRequest(model);
        }

        public void AddError(IList<CommandMessageDTO> model, string resourceName, params object[] args)
        {
            model.Add(new CommandMessageDTO
            {
                ResourceName = resourceName,
                Arguments = args
            });
        }
    }
}
