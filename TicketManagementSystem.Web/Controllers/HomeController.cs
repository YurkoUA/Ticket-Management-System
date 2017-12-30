using System.Web.Mvc;
using System.Web.UI;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPackageService _packageService;
        private readonly ITicketService _ticketService;

        public HomeController(ITicketService ticketService, IPackageService packageService)
        {
            _ticketService = ticketService;
            _packageService = packageService;
        }

        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var ticketsCount = _ticketService.GetCount();

            ViewBag.TotalCount = ticketsCount.Total;
            ViewBag.TotalOfHappy = ticketsCount.Happy;
            ViewBag.TotalPackages = _packageService.TotalCount;
            return View();
        }
    }
}