using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    public class SummaryService : Service, ISummaryService
    {
        private readonly IPackageService _packageService;
        private readonly ITicketService _ticketService;

        public SummaryService(IUnitOfWork database, IPackageService packServ, ITicketService ticketServ) : base(database)
        {
            _packageService = packServ;
            _ticketService = ticketServ;
        }

        public IEnumerable<SummaryDTO> GetSummaries()
        {
            return MapperInstance.Map<IEnumerable<SummaryDTO>>(
                Database.Summary.GetAll()
                .AsEnumerable());
        }

        public IEnumerable<SummaryPeriodDTO> GetSummariesPeriods()
        {
            var periods = new List<SummaryPeriodDTO>();
            var summaries = Database.Summary.GetAll()
                .OrderBy(s => s.Date)
                .ToList();

            for (int i = 1; i < summaries.Count(); i++)
            {
                periods.Add(new SummaryPeriodDTO(summaries[i - 1], summaries[i]));
            }

            AddCurrentPeriod(periods, summaries.Last());

            return periods;
        }

        public void WriteSummary()
        {
            Database.Summary.Create(GetSummary());
            Database.SaveChanges();
        }

        public bool ExistsByCurrentMonth()
        {
            return Database.Summary.GetAll().AsEnumerable().Any(isThisMonth);

            bool isThisMonth(Summary summary)
            {
                var now = DateTime.Now.ToUniversalTime();

                return summary.Date.Month == now.Month
                    && summary.Date.Year == now.Year;
            }
        }

        private Summary GetSummary()
        {
            return new Summary
            {
                Date = DateTime.UtcNow,
                Packages = _packageService.TotalCount,
                Tickets = _ticketService.TotalCount,
                HappyTickets = _ticketService.CountHappyTickets()
            };
        }

        private void AddCurrentPeriod(ICollection<SummaryPeriodDTO> periods, Summary lastSummary)
        {
            var current = GetSummary();

            if (current.Tickets - lastSummary.Tickets > 0)
            {
                periods.Add(new SummaryPeriodDTO(lastSummary, current));
            }
        }
    }
}
