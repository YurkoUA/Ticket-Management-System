using System.Web.Mvc;

namespace TicketManagementSystem.Web.Areas.Todo
{
    public class TodoAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Todo";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    $"{AreaName}_default",
            //    AreaName + "/{controller}/{action}/{id}",
            //    new { id = UrlParameter.Optional },
            //    namespaces: new[] { "TicketManagementSystem.Web.Areas.Todo.Controllers" }
            //);
        }
    }
}