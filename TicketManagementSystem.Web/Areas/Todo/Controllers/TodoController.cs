using System.Web.Mvc;

namespace TicketManagementSystem.Web.Areas.Todo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TodoController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Areas/Todo/Views/Todo/Index.cshtml");
        }
    }
}