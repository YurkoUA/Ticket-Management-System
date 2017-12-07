using Ninject;
using Quartz;
using TicketManagementSystem.Business.AppSettings;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Business.Telegram;
using TicketManagementSystem.Web.App_Start;

namespace TicketManagementSystem.Web.Jobs
{
    public class SummaryJob : IJob
    {
        private ISummaryService _summaryService;
        private IAppSettingsService _appSettingsService;
        private ITelegramNotificationService _telegramNotificationService;

        public void Execute(IJobExecutionContext context)
        {
            var ninjectInstance = NinjectWebCommon.GetInstance();

            _summaryService = ninjectInstance.Get<ISummaryService>();
            _appSettingsService = ninjectInstance.Get<IAppSettingsService>();
            _telegramNotificationService = ninjectInstance.Get<ITelegramNotificationService>();

            if (!_appSettingsService.IsAutomaticSummariesEnabled)
                return;

            try
            {
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