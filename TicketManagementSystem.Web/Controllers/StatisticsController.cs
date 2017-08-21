using System.Web.Mvc;
using System.Web.UI;

namespace TicketManagementSystem.Web.Controllers
{
    [OutputCache(Duration = 30, Location = OutputCacheLocation.ServerAndClient)]
    public class StatisticsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}