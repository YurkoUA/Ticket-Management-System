using System.Web.Mvc;

namespace TicketManagementSystem.Web.Areas.Api
{
    public class ApiAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Api";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "Api_default",
            //    "api/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "TicketManagementSystem.Web.Areas.Api.Controllers" }
            //);
        }
    }
}