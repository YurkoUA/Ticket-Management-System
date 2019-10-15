using System.Collections.Generic;
using System.Linq;

namespace TicketManagementSystem.Domain.DTO
{
    public class CommandResultDTO<TModel>
    {
        public bool IsSuccess => !Errors.Any();
        public TModel Model { get; set; }
        public IList<CommandMessageDTO> Errors { get; set; } = new List<CommandMessageDTO>();
    }
}
