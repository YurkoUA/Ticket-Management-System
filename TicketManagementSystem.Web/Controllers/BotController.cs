using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    public class BotController : Controller
    {
        private ITicketService _ticketService;
        private IPackageService _packageService;

        public BotController(ITicketService ticketSerivice, IPackageService packageService)
        {
            _packageService = packageService;
            _ticketService = ticketSerivice;
        }

        [HttpGet, OutputCache(Duration = 20, Location = OutputCacheLocation.Server)]
        public JsonResult Total()
        {
            var response = new
            {
                totalTickets = _ticketService.TotalCount,
                happyTickets = _ticketService.CountHappyTickets(),
                totalPackages = _packageService.TotalCount
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, OutputCache(Duration = 20, Location = OutputCacheLocation.Server)]
        public JsonResult Number(string number)
        {
            var tickets = _ticketService.GetByNumber(number, true)
                .Select(t => new
                {
                    Number = t.Number,
                    Color = t.ColorName,
                    Serial = t.SerialName + t.SerialNumber,
                    Package = t.PackageName
                });
            return Json(tickets, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, OutputCache(Duration = 20, Location = OutputCacheLocation.Server)]
        public JsonResult Last()
        {
            var tickets = _ticketService.GetTickets()
                .OrderByDescending(t => t.Id)
                .Take(10)
                .Select(t => new
                {
                    Number = t.Number,
                    Color = t.ColorName,
                    Serial = t.SerialName + t.SerialNumber,
                    Package = t.PackageName
                });
            return Json(tickets, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Random()
        {
            var index = new Random().Next(0, _ticketService.TotalCount);
            var ticket = _ticketService.GetTickets(index, 1)
                .FirstOrDefault();

            if (ticket == null)
                return null;

            var response = new
            {
                Number = ticket.Number,
                Color = ticket.ColorName,
                Serial = ticket.SerialName + ticket.SerialNumber,
                Package = ticket.PackageName
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}