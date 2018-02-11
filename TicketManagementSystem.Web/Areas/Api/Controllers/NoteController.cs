using System.Web.Http;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    [RoutePrefix("api/Note")]
    public class NoteController : BaseApiController
    {
        private readonly ITicketService2 _ticketService2;

        public NoteController(ITicketService2 ticketService2)
        {
            _ticketService2 = ticketService2;
        }

        [HttpGet]
        public IHttpActionResult Get(string note, int take = 5)
        {
            if (string.IsNullOrEmpty(note))
                return BadRequest();

            if (take < 1)
                take = 5;

            return OkOrNoContent(_ticketService2.GetNotes(note, take));
        }
    }
}
