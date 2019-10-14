using System.Collections.Generic;

namespace TicketManagementSystem.Domain.DTO
{
    public class CommandResultDTO<TModel>
    {
        public bool IsSuccess { get; set; } = true;
        public TModel Model { get; set; }
        public IList<CommandMessageDTO> Errors { get; set; }
    }
}
