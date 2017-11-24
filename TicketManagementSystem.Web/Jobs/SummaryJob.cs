using System;
using System.Web.Configuration;
using Ninject;
using Quartz;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Business.Telegram;
using TicketManagementSystem.Web.App_Start;

namespace TicketManagementSystem.Web.Jobs
{
    public class SummaryJob : IJob
    {
        private ISummaryService _summaryService;
        private ITelegramNotificationService _telegramNotificationService;

        public void Execute(IJobExecutionContext context)
        {
            if (!Convert.ToBoolean(WebConfigurationManager.AppSettings["AutomaticSummariesEnabled"]))
                return;

            try
            {
                var ninjectInstance = NinjectWebCommon.GetInstance();
                _summaryService = ninjectInstance.Get<ISummaryService>();
                _telegramNotificationService = ninjectInstance.Get<ITelegramNotificationService>();

                if (!_summaryService.ExistsByCurrentMonth())
                {
                    _summaryService.WriteSummary();
                    _telegramNotificationService.TrySendMessage("Підсумок за місяць успішно зафіксовано!");
                }
            }
            catch
            {
                _telegramNotificationService.TrySendMessage("Помилка створення місячного підсумку.");
            }
        }
    }
}