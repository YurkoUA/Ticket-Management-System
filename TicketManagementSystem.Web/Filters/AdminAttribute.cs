using System.Web;
using System.Web.Mvc;
using TicketManagementSystem.Business.Infrastructure;

namespace TicketManagementSystem.Web.Filters
{
    public class AdminAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext) => httpContext.User.IsAdmin();
    }
}