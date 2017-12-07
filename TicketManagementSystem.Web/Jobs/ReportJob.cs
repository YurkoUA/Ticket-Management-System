using Ninject;
using Quartz;
using TicketManagementSystem.Business.AppSettings;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Business.Telegram;
using TicketManagementSystem.Web.App_Start;

namespace TicketManagementSystem.Web.Jobs
{
    public class ReportJob : IJob
    {
        private IReportService _reportService;
        private IAppSettingsService _appSettingsService;
        private ITelegramNotificationService _telegramNotificationService;

        public void Execute(IJobExecutionContext context)
        {
            var ninjectInstance = NinjectWebCommon.GetInstance();

            _reportService = ninjectInstance.Get<IReportService>();
            _appSettingsService = ninjectInstance.Get<IAppSettingsService>();
            _telegramNotificationService = ninjectInstance.Get<ITelegramNotificationService>();

            if (!_appSettingsService.IsAutomaticReportsEnabled)
                return;

            var reportsDirectory = context.JobDetail.JobDataMap["reportsDirectory"] as string;
            var baseUrl = context.JobDetail.JobDataMap["baseUrl"] as string;

            if (string.IsNullOrEmpty(reportsDirectory) || string.IsNullOrEmpty(baseUrl))
                return;

            var report = _reportService.CreateReport(true);

            var isSuccess = _reportService.TryCreatePDFs(report,
                action => $"{baseUrl}Report/{action}",
                fileName => $"{reportsDirectory}{fileName}.pdf");

            if (!isSuccess)
            {
                _telegramNotificationService.TrySendMessage("Помилка створення автоматичних звітів.");
                return;
            }

            _reportService.SaveReport(report);
            _telegramNotificationService.TrySendMessage("Автоматичні звіти успішно створено!");
        }
    }
}