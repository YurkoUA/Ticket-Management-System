using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketManagementSystem.Web.Filters
{
    public class AdminAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
        }
    }
}