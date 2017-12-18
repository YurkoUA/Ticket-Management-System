using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.DTO.Report;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business.Services
{
    public class TicketService2 : Service, ITicketService2
    {
        private readonly IReportService _reportService;

        public TicketService2(IUnitOfWork database, IReportService reportService) : base(database)
        {
            _reportService = reportService;
        }
        
        public IEnumerable<TicketDTO> GetLatestTickets()
        {
            if (_reportService.IsEmpty)
                return null;

            var date = _reportService.GetLastReport().Date;

            return MapperInstance.Map<IEnumerable<TicketDTO>>(
                Database.Tickets.GetAll(t => t.AddDate > date)
                .OrderBy(t => t.Number)
                .AsEnumerable());
        }

        public IEnumerable<TicketDTO> GetTodayTickets(int timezoneOffset)
        {
            timezoneOffset *= -1;

            var tickets = Database.Tickets
                .GetAllWithInclude(t => t.AddDate.AddMinutes(timezoneOffset).Date == DateTime.UtcNow.AddMinutes(timezoneOffset).Date)
                .OrderBy(t => t.Number)
                .AsEnumerable();
            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketGroupDTO> GetSummaryByLatest()
        {
            var tickets = GetLatestTickets();

            return tickets.GroupBy(t => $"{t.SerialName}-{t.ColorName} ({t.FirstNumber})").Select(g => new TicketGroupDTO
            {
                Name = g.Key,
                Count = g.Count(),
                HappyCount = g.Count(t => t.IsHappy)
            }).OrderByDescending(t => t.Count);
        }
    }
}
