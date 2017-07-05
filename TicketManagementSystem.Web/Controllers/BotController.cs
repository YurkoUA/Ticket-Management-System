using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    public class BotController : ApplicationController
    {
        private ITicketService _ticketService;
        private IPackageService _packageService;

        public BotController(ITicketService ticketSerivice, IPackageService packageService)
        {
            _packageService = packageService;
            _ticketService = ticketSerivice;
        }

        [HttpGet, OutputCache(Duration = 20, Location = OutputCacheLocation.Server)]
        public ActionResult Total()
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
        public ActionResult Number(string number)
        {
            if (!Regex.IsMatch(number, @"\d{6}") || number == null)
                return HttpBadRequest();

            var tickets = _ticketService.GetByNumber(number, true);

            if (!tickets.Any())
                return HttpNotFound();

            var response = tickets.Select(t => new
            {
                Id = t.Id,
                Number = t.Number,
                Color = t.ColorName,
                Serial = t.SerialName + t.SerialNumber,
                Package = t.PackageName,
                Url = Url.Action("Details", "Ticket", new { id = t.Id })
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Random()
        {
            var index = new Random().Next(0, _ticketService.TotalCount);
            var ticket = _ticketService.GetTickets(index, 1)
                .FirstOrDefault();

            if (ticket == null)
                return HttpNotFound();

            var response = new
            {
                Id = ticket.Id,
                Number = ticket.Number,
                Color = ticket.ColorName,
                Serial = ticket.SerialName + ticket.SerialNumber,
                Package = ticket.PackageName,
                Url = Url.Action("Details", "Ticket", new { id = ticket.Id })
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}