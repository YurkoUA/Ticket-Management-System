using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.Extensions;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Web.Controllers;

namespace TicketManagementSystem.Web.Areas.Report.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Model is last report.
            return View(_reportService.GetLastReport());
        }

        [HttpGet, OutputCache(Duration = 30, Location = OutputCacheLocation.Client)]
        public ActionResult Archive()
        {
            var reports = _reportService.GetReports()
                .OrderByDescending(r => r.Date)
                .GroupBy(r => r.Date.ToString("MMMM yyyy"))
                .ToDictionary(g => g.Key.FirstCharToUpper(), g => g.AsEnumerable());

            return View(reports);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public ActionResult CreateReport()
        {
            var report = _reportService.CreateReport(false);
            var folderPath = Request.RequestContext.HttpContext.Server.MapPath("~/Files/Reports/");

            var isSuccess = _reportService.TryCreatePDFs(report,
                    action => Request.Url.Authority + Url.Action(action),
                    fileName => $"{folderPath}{fileName}.pdf");

            if (!isSuccess)
                return HttpBadRequest();

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