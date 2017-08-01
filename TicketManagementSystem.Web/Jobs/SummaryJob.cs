using Ninject;
using Quartz;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Web.App_Start;

namespace TicketManagementSystem.Web.Jobs
{
    public class SummaryJob : IJob
    {
        private ISummaryService _summaryService;

        public void Execute(IJobExecutionContext context)
        {
            _summaryService = NinjectWebCommon.GetInstance().Get<ISummaryService>();

            if (!_summaryService.ExistsByCurrentMonth())
                _summaryService.WriteSummary();
        }
    }
}