using System.Linq;
using System.Web.Http;
using TicketManagementSystem.Business.DTO.Report;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    [RoutePrefix("api/Report")]
    public class ReportController : BaseApiController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IHttpActionResult GetAll()
        {
            var reports = _reportService.GetReports();

            if (!reports.Any())
                return NoContent();

            return Ok(reports.Select(r => ReportToResponse(r)));
        }

        public IHttpActionResult Get(int id)
        {
            var report = _reportService.GetById(id);

            if (report == null)
                return NotFound();

            return Ok(ReportToResponse(report));
        }

        public IHttpActionResult GetLastReport()
        {
            var report = _reportService.GetLastReport();

            if (report == null)
                return NoContent();

            return Ok(ReportToResponse(report));
        }

        private dynamic ReportToResponse(ReportDTO report)
        {
            return new
            {
                Id = report.Id,
                Date = report.Date,
                IsAuthomatic = report.IsAutomatic,
                DefaultReport = MapUrl(report.DefaultReportFileName()),
                PackagesReport = MapUrl(report.PackagesReportFileName())
            };
        }

        private string MapUrl(string filePath)
        {
            return $"{Request.RequestUri.Scheme}://{Request.RequestUri.Authority}/Files/Reports/{filePath}.pdf";
        }
    }
}
