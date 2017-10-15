using System.Collections;
using System.Linq;
using System.Web.Http;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    [RoutePrefix("api/Statistics")]
    public class StatisticsController : BaseApiController
    {
        private readonly IColorService _colorService;
        private readonly ISerialService _serialService;
        private readonly ITicketService _ticketService;
        private readonly ISummaryService _summaryService;

        public StatisticsController(IColorService colorServ,
                                    ISerialService serialServ,
                                    ITicketService ticketServ,
                                    ISummaryService summaryServ)
        {
            _colorService = colorServ;
            _serialService = serialServ;
            _ticketService = ticketServ;
            _summaryService = summaryServ;
        }

        [HttpGet]
        public IHttpActionResult Tickets()
        {
            var happy = _ticketService.CountHappyTickets();

            return Ok(new ArrayList
            {
                new { Type = "Звичайні", Count = _ticketService.TotalCount - happy },
                new { Type = "Щасливі", Count = happy }
            });
        }

        [HttpGet]
        public IHttpActionResult ByFirstNumber()
        {
            return Ok(_ticketService.GetTickets()
                .GroupBy(t => t.FirstNumber)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Number = g.Key.ToString(),
                    Count = g.Count()
                }));
        }

        [HttpGet]
        public IHttpActionResult HappyByFirstNumber()
        {
            return Ok(_ticketService.GetHappyTickets()
                .GroupBy(t => t.FirstNumber)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Number = g.Key.ToString(),
                    Count = g.Count()
                }));
        }

        [HttpGet]
        public IHttpActionResult BySerial()
        {
            return Ok(_serialService.GetSeries()
                .OrderByDescending(s => s.TicketsCount)
                .Select(s => new
                {
                    Serial = s.Name,
                    Tickets = s.TicketsCount
                }));
        }

        [HttpGet]
        public IHttpActionResult ByColor()
        {
            return Ok(_colorService.GetColors()
                .OrderByDescending(c => c.TicketsCount)
                .Select(c => new
                {
                    Color = c.Name,
                    Tickets = c.TicketsCount
                }));
        }

        [HttpGet]
        public IHttpActionResult HappyBySerial()
        {
            return Ok(_ticketService.GetHappyTickets()
                .GroupBy(t => t.SerialName)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    Serial = g.Key,
                    Tickets = g.Count()
                }));
        }

        [HttpGet]
        public IHttpActionResult Summaries()
        {
            return OkOrNoContent(_summaryService.GetSummaries());
        }

        [HttpGet]
        public IHttpActionResult SummariesPeriods()
        {
            return OkOrNoContent(_summaryService.GetSummariesPeriods());
        }
    }
}
