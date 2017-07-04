using System;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    public class SummaryService : Service, ISummaryService
    {
        private IPackageService _packageService;
        private ITicketService _ticketService;

        public SummaryService(IUnitOfWork database, IPackageService packServ, ITicketService ticketServ) : base(database)
        {
            _packageService = packServ;
            _ticketService = ticketServ;
        }

        public void WriteSummary()
        {
            var summary = new Summary
            {
                Date = DateTime.Now.ToUniversalTime(),
                Packages = _packageService.TotalCount,
                Tickets = _ticketService.TotalCount,
                HappyTickets = _ticketService.CountHappyTickets()
            };

            Database.Summary.Create(summary);
            Database.SaveChanges();
        }

        public bool ExistsByCurrentMonth()
        {
            return Database.Summary.Contains(s => isThisMonth(s.Date));

            bool isThisMonth(DateTime date)
            {
                var now = DateTime.Now.ToUniversalTime();

                return date.Month == now.Month
                    && date.Year == now.Year;
            }
        }
    }
}
