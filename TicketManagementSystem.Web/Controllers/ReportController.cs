using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly IPdfService _pdfService;

        public ReportController(IReportService reportService, IPdfService pdfService)
        {
            _reportService = reportService;
            _pdfService = pdfService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new ReportIndexModel
            {
                ArchiveIsEmpty = _reportService.IsEmpty,
                LastReport = _reportService.GetLastReport()
            });
        }

        [HttpGet, OutputCache(Duration = 30, Location = OutputCacheLocation.Client)]
        public ActionResult Archive()
        {
            var reports = _reportService.GetReports()
                .OrderByDescending(r => r.Id);

            if (!reports.Any())
                return HttpNotFound();

            return PartialView("ArchiveModal", reports);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public ActionResult CreateReport()
        {
            var report = _reportService.CreateReport(false);
            var folderPath = Request.RequestContext.HttpContext.Server.MapPath("~/Files/Reports/");

            try
            {
                Func<string, string> fullUrl = actionName => Request.Url.Authority + Url.Action(actionName);
                Func<string, string> pdfPath = fileName => $"{folderPath}{fileName}.pdf";

                _pdfService.CreatePdf(fullUrl("DefaultReportPrint"), pdfPath(report.DefaultReportFileName()));
                _pdfService.CreatePdf(fullUrl("PackagesReportPrint"), pdfPath(report.PackagesReportFileName()));
            }
            catch
            {
                return HttpBadRequest();
            }

            _reportService.SaveReport(report);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DefaultReportPrint()
        {
            return View(_reportService.GetDefaultReportDTO());
        }

        [HttpGet]
        public ActionResult PackagesReportPrint()
        {
            return View(_reportService.GetPackagesReportDTO());
        }
    }
}