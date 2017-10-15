using System;
using System.Web.Configuration;
using Ninject;
using Quartz;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Web.App_Start;

namespace TicketManagementSystem.Web.Jobs
{
    public class ReportJob : IJob
    {
        private IReportService _reportService;
        private IPdfService _pdfService;

        public void Execute(IJobExecutionContext context)
        {
            _reportService = NinjectWebCommon.GetInstance().Get<IReportService>();
            _pdfService = NinjectWebCommon.GetInstance().Get<IPdfService>();

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
                return;
            }

            _reportService.SaveReport(report);
        }
    }
}