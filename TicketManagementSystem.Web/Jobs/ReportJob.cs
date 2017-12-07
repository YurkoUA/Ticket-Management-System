using System;
using System.Web.Configuration;
using Ninject;
using Quartz;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Business.Telegram;
using TicketManagementSystem.Web.App_Start;

namespace TicketManagementSystem.Web.Jobs
{
    public class ReportJob : IJob
    {
        private IReportService _reportService;
        private ITelegramNotificationService _telegramNotificationService;

        public void Execute(IJobExecutionContext context)
        {
            var ninjectInstance = NinjectWebCommon.GetInstance();

            _reportService = ninjectInstance.Get<IReportService>();
            _telegramNotificationService = ninjectInstance.Get<ITelegramNotificationService>();

            if (!Convert.ToBoolean(WebConfigurationManager.AppSettings["AutomaticReportsEnabled"]))
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