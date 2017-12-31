using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ITicketNotesService
    {
        IEnumerable<TicketNotesDTO> GetNotes();
        IEnumerable<TicketDTO> GetTicketsByNote(string note);
    }
}
