using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business.Services
{
    public class TicketNotesService : Service, ITicketNotesService
    {
        public TicketNotesService(IUnitOfWork database) : base(database)
        {
        }

        public IEnumerable<TicketNotesDTO> GetNotes()
        {
            return Database.Tickets.GetAllIncluding(t => !string.IsNullOrEmpty(t.Note),
                t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Color, t => t.Package.Serial, t => t.Package.Tickets)
                .GroupBy(t => t.Note)
                .OrderByDescending(g => g.Count())
                .Select(g => new TicketNotesDTO
                {
                    Note = g.Key,
                    TicketsCount = g.Count()
                });
        }

        public IEnumerable<TicketDTO> GetTicketsByNote(string note)
        {
            var tickets = Database.Tickets.GetAllIncluding(t => t.Note.Equals(note),
                t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Color, t => t.Package.Serial, t => t.Package.Tickets)
                .OrderBy(t => t.Number);

            return Mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }
    }
}
