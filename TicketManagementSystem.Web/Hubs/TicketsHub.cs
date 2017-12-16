using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace TicketManagementSystem.Web.Hubs
{
    [HubName("ticketsHub")]
    public class TicketsHub : Hub
    {
        private static IHubContext _context;

        static TicketsHub()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<TicketsHub>();
        }

        public static void RemoveTicketsIds(IEnumerable<int> ticketsId)
        {
            _context.Clients.All.removeTickets(ticketsId);
        }
    }
}