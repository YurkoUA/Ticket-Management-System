using System.Web.Http;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    [RoutePrefix("api/Note")]
    public class NoteController : BaseApiController
    {
        private readonly ITicketNotesService _ticketNotesService;

        public NoteController(ITicketNotesService ticketNotesService)
        {
            _ticketNotesService = ticketNotesService;
        }

        [HttpGet]
        public IHttpActionResult Get(string note, int take = 5)
        {
            if (string.IsNullOrEmpty(note))
                return BadRequest();

            if (take < 1)
                take = 5;

            return OkOrNoContent(_ticketNotesService.GetNotes(note, take));
        }
    }
}
