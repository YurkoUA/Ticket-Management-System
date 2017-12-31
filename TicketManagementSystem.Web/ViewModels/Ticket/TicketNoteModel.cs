using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TicketManagementSystem.Web
{
    public class TicketNoteModel : IEnumerable<TicketDetailsModel>
    {
        public string Note { get; set; }
        public IEnumerable<TicketDetailsModel> Tickets { get; set; }
        public int TicketsCount => Tickets.Count();

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