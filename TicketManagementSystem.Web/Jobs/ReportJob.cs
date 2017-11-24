using System;
using System.Web.Configuration;
using Ninject;
using Quartz;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Business.Telegram;
using TicketManagementSystem.Web.App_Start;

namespace TicketManagementSystem.Web.Jobs
{
    public class ReportJob : IJob
    {
        private IReportService _reportService;
        private IPdfService _pdfService;
        private ITelegramNotificationService _telegramNotificationService;

        public void Execute(IJobExecutionContext context)
        {
            var ninjectInstance = NinjectWebCommon.GetInstance();
            _reportService = ninjectInstance.Get<IReportService>();
            _pdfService = ninjectInstance.Get<IPdfService>();
            _telegramNotificationService = ninjectInstance.Get<ITelegramNotificationService>();

            if (!Convert.ToBoolean(WebConfigurationManager.AppSettings["AutomaticReportsEnabled"]))
                return;

            var reportsDirectory = context.JobDetail.JobDataMap["reportsDirectory"] as string;
            var baseUrl = context.JobDetail.JobDataMap["baseUrl"] as string;

            if (string.IsNullOrEmpty(reportsDirectory) || string.IsNullOrEmpty(baseUrl))
                return;

            var report = _reportService.CreateReport(true);

            Func<string, string> mapActionUrl = action => $"{baseUrl}Report/{action}";
            Func<string, string> mapPdfPath = fileName => $"{reportsDirectory}{fileName}.pdf";

            try
            {
                _pdfService.CreatePdf(mapActionUrl("DefaultReportPrint"), mapPdfPath(report.DefaultReportFileName()));
                _pdfService.CreatePdf(mapActionUrl("PackagesReportPrint"), mapPdfPath(report.PackagesReportFileName()));
            }
            catch
            {
                _telegramNotificationService.TrySendMessage("Помилка створення автоматичних звітів.");

                return;
            }

            _reportService.SaveReport(report);
            _telegramNotificationService.TrySendMessage("Автоматичні звіти успішно створено!");
        }
    }
}