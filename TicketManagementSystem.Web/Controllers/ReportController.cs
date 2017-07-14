using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : ApplicationController
    {
        private IReportService _reportService;
        private IPdfService _pdfService;

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

        [HttpGet]
        public ActionResult Archive()
        {
            var reports = _reportService.GetReports()
                .OrderByDescending(r => r.Id);

            if (!reports.Any())
                return HttpNotFound();

            return PartialView("ArchiveModal", reports);
        }

        [HttpGet]
        public ActionResult CreateReport(bool isAutomatic = false)
        {
            var report = _reportService.CreateReport(isAutomatic);
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

        [HttpGet, AllowAnonymous]
        public ActionResult DefaultReportPrint()
        {
            return View(_reportService.GetDefaultReportDTO());
        }

        [HttpGet, AllowAnonymous]
        public ActionResult PackagesReportPrint()
        {
            return View(_reportService.GetPackagesReportDTO());
        }
    }
}