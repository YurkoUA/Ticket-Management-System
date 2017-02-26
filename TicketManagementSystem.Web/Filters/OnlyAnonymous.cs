using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace TicketManagementSystem.Web.Filters
{
    public class OnlyAnonymous : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var user = filterContext.HttpContext.User;

            if (user.Identity.IsAuthenticated)
                filterContext.Result = new HttpUnauthorizedResult();
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "controller", "Home" }, { "action", "Index" }
                });
            }
        }
    }
}