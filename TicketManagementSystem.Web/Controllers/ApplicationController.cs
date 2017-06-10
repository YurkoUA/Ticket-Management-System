using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using AutoMapper;

namespace TicketManagementSystem.Web.Controllers
{
    public class ApplicationController : Controller
    {
        protected IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        protected IMapper MapperInstance => AutoMapperConfig.CreateMapper();

        protected HttpStatusCodeResult HttpBadRequest() => new HttpStatusCodeResult(400);

        protected ActionResult ErrorPartial(ModelStateDictionary modelState)
        {
            return PartialView("ErrorListPartial", GetModelStateErrors(modelState));
        }

        protected ActionResult SuccessAlert(string message)
        {
            ViewBag.Message = message;
            return PartialView("SuccessAlertPartial");
        }

        protected ActionResult SuccessAlert(string message, string url, string urlMessage)
        {
            ViewBag.Message = message;
            ViewBag.Url = url;
            ViewBag.UrlMessage = urlMessage;

            return PartialView("SuccessAlertLinkPartial");
        }

        protected string[] GetModelStateErrors(ModelStateDictionary modelState)
        {
            return modelState.Values.Where(e => e.Errors.Count > 0)
                    .SelectMany(e => e.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();
        }
    }
}