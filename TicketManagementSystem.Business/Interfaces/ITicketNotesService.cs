using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ITicketNotesService
    {
        IEnumerable<TicketNotesDTO> GetNotes();
        IEnumerable<TicketDTO> GetTicketsByNote(string note);
        IEnumerable<string> GetNotes(string note, int take);
    }
}
