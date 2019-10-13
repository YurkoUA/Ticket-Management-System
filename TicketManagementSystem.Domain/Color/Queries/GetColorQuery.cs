using TicketManagementSystem.Infrastructure.Domain;
using TicketManagementSystem.ViewModels.Color;

namespace TicketManagementSystem.Domain.Color.Queries
{
    public class GetColorQuery : IQuery<ColorVM>
    {
        public int ColorId { get; set; }
    }
}
