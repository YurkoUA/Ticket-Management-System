using System.Web.Mvc;

namespace TicketManagementSystem.Web.Areas.Report
{
    public class ReportAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Report";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Report_default",
                "Report/{action}/{id}",
                new { controller = "Report", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}