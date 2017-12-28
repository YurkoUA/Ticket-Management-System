using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface IStatisticsService
    {
        IEnumerable<ChartDTO> TicketsBySerial();
        IEnumerable<ChartDTO> TicketsByColor();
        IEnumerable<ChartDTO> TicketsByFirstNumber();
        IEnumerable<ChartDTO> TicketsTypes();

        IEnumerable<ChartDTO> HappyTicketsBySerial();
        IEnumerable<ChartDTO> HappyTicketsByFirstNumber();
    }
}
