using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using AutoMapper;
using TicketManagementSystem.ViewModels.Common;

namespace TicketManagementSystem.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        protected IMapper Mapper => AutoMapperConfig.GetInstance();

        protected HttpStatusCodeResult HttpBadRequest() => new HttpStatusCodeResult(400);
        protected HttpStatusCodeResult HttpSuccess() => new HttpStatusCodeResult(200);

        protected ActionResult ErrorPartial(ModelStateDictionary modelState)
        {
            return PartialView("ErrorListPartial", modelState.ToEnumerableString());
        }

        protected ActionResult ErrorPartial<TModel>(CommandResultVM<TModel> result)
        {
            return PartialView("ErrorListPartial", result.Errors);
        }

        protected ActionResult SuccessPartial(string message)
        {
            ViewBag.Message = message;
            return PartialView("SuccessAlertPartial");
        }

        protected ActionResult SuccessPartial(string message, string url, string urlMessage)
        {
            ViewBag.Message = message;
            ViewBag.Url = url;
            ViewBag.UrlMessage = urlMessage;

            return PartialView("SuccessAlertLinkPartial");
        }
    }
}