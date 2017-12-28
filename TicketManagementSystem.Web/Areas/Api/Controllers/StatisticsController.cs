using System.Web.Http;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    [RoutePrefix("api/Statistics")]
    public class StatisticsController : BaseApiController
    {
        private readonly ISummaryService _summaryService;
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService, ISummaryService summaryService)
        {
            _statisticsService = statisticsService;
            _summaryService = summaryService;
        }

        [HttpGet]
        public IHttpActionResult Tickets() => Ok(_statisticsService.TicketsTypes());

        [HttpGet]
        public IHttpActionResult ByFirstNumber() => Ok(_statisticsService.TicketsByFirstNumber());

        [HttpGet]
        public IHttpActionResult HappyByFirstNumber() => Ok(_statisticsService.HappyTicketsByFirstNumber());

        [HttpGet]
        public IHttpActionResult BySerial() => Ok(_statisticsService.TicketsBySerial());

        [HttpGet]
        public IHttpActionResult ByColor() => Ok(_statisticsService.TicketsByColor());

        [HttpGet]
        public IHttpActionResult HappyBySerial() => Ok(_statisticsService.HappyTicketsBySerial());

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
