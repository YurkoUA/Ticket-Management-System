using Quartz;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Jobs
{
    public class SummaryJob : IJob
    {
        private ISummaryService _summaryService;

        public SummaryJob(ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        public void Execute(IJobExecutionContext context)
        {
            if (!_summaryService.ExistsByCurrentMonth())
                _summaryService.WriteSummary();
        }
    }
}