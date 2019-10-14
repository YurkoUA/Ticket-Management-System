using System.Collections.Generic;

namespace TicketManagementSystem.ViewModels.Common
{
    public class CommandResultVM<TModel>
    {
        public bool IsSuccess { get; set; }
        public TModel Model { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
