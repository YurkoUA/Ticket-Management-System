using System.Collections.Generic;
using System.Web.Mvc;

namespace TicketManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class TicketController : BaseController
    {
        [HttpGet, AllowAnonymous]
        public ActionResult Notes() => View(_ticketNotesService.GetNotes());

        [HttpGet, AllowAnonymous]
        public ActionResult Note(string note)
        {
            if (string.IsNullOrEmpty(note))
                return HttpBadRequest();

            var tickets = _ticketNotesService.GetTicketsByNote(note);

            var viewModel = new TicketNoteModel
            {
                Note = note,
                Tickets = Mapper.Map<IEnumerable<TicketDetailsModel>>(tickets)
            };
            return View(viewModel);
        }
    }
}