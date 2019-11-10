﻿using System.Threading.Tasks;
using System.Web.Http;
using TicketManagementSystem.Domain.Statistics.Queries;
using TicketManagementSystem.Infrastructure.Domain.Processors;

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
    }
}