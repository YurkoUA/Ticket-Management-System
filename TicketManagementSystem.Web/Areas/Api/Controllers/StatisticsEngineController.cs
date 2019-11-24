using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TicketManagementSystem.Domain.Cqrs;
using TicketManagementSystem.Domain.Statistics.Queries;
using TicketManagementSystem.Infrastructure.Domain.Processors;
using TicketManagementSystem.ViewModels.Statistics;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    [RoutePrefix("api/Statistics")]
    public class StatisticsEngineController : BaseApiController
    {
        private readonly IQueryProcessorAsync queryProcessorAsync;

        public StatisticsEngineController(IQueryProcessorAsync queryProcessorAsync)
        {
            this.queryProcessorAsync = queryProcessorAsync;
        }

        [HttpGet]
        [Route("Charts/{pageId?}")]
        public async Task<IHttpActionResult> Charts([FromUri]GetChartsQuery query)
        {
            var charts = await queryProcessorAsync.ProcessAsync(query ?? new GetChartsQuery());
            return Ok(charts);
        }

        [HttpGet]
        [Route("Data")]
        public async Task<IHttpActionResult> Data([FromUri]GetChartDataQuery query)
        {
            var data = await queryProcessorAsync.ProcessAsync(query);
            return Ok(data);
        }

        [HttpGet]
        [Route("Pages")]
        public async Task<IHttpActionResult> Pages()
        {
            var pages = await queryProcessorAsync.ProcessAsync(new EmptyQuery<IEnumerable<PageVM>>());
            return Ok(pages);
        }
    }
}