using System.Web.Mvc;
using System.Web.UI;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private IPackageService _packageService;
        private ITicketService _ticketService;

        public HomeController(ITicketService ticketService, IPackageService packageService)
        {
            _ticketService = ticketService;
            _packageService = packageService;
        }

        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            ViewBag.TotalCount = _ticketService.TotalCount;
            ViewBag.TotalOfHappy = _ticketService.CountHappyTickets();
            ViewBag.TotalPackages = _packageService.TotalCount;
            return View();
        }
    }
}