using System.Collections;
using System.Collections.Generic;

namespace TicketManagementSystem.Web
{
    public class TicketIndexModel : IEnumerable<TicketDetailsModel>
    {
        public IEnumerable<TicketDetailsModel> Tickets { get; set; }
        public PageInfo PageInfo { get; set; }

        public IEnumerator<TicketDetailsModel> GetEnumerator()
        {
            return Tickets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Tickets.GetEnumerator();
        }
    }
}