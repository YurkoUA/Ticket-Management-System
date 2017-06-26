using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json.Linq;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    [OutputCache(Duration = 15, Location = OutputCacheLocation.Server)]
    public class StatisticsController : Controller
    {
        private IColorService _colorService;
        private ISerialService _serialService;
        private ITicketService _ticketService;

        public StatisticsController(IColorService colorServ, ISerialService serialServ, ITicketService ticketServ)
        {
            _colorService = colorServ;
            _serialService = serialServ;
            _ticketService = ticketServ;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ContentResult Series()
        {
            var responseArray = new JArray
            {
                new JArray { "Chart", "Tickets by series" }
            };

            _serialService.GetSeries()
                .OrderByDescending(s => s.TicketsCount)
                .ToList()
                .ForEach(s =>
                {
                    responseArray.Add(new JArray { s.Name, s.TicketsCount });
                });

            return Content(responseArray.ToString());
        }

        [HttpGet]
        public ContentResult Colors()
        {
            var responseArray = new JArray
            {
                new JArray { "Chart", "Tickets by colors" }
            };

            _colorService.GetColors()
                .OrderByDescending(c => c.TicketsCount)
                .ToList()
                .ForEach(c =>
                {
                    responseArray.Add(new JArray{c.Name, c.TicketsCount });
                });

            return Content(responseArray.ToString());
        }

        [HttpGet]
        public ContentResult HappyTickets()
        {
            var happy = _ticketService.CountHappyTickets();

            var json = new JArray
            {
                new JArray {"Chart", "Tickets" },
                new JArray {"Звичайні", _ticketService.TotalCount - happy },
                new JArray { "Щасливі", happy }
            };
            return Content(json.ToString());
        }

        [HttpGet]
        public ContentResult TicketsByFirstNumber()
        {
            var json = new JArray
            {
                new JArray {"Chart", "Tickets by number" }
            };

            _ticketService.GetTickets()
                .GroupBy(t => t.FirstNumber)
                .OrderBy(g => g.Key)
                .ToList()
                .ForEach(n =>
                {
                    json.Add(new JArray { n.Key.ToString(), n.Count() });
                });

            return Content(json.ToString());
        }
    }
}