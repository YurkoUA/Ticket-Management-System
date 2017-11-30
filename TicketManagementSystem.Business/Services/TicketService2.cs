﻿using System.Collections.Generic;
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
                .OrderBy(t => t.Number));
        }

        public IEnumerable<TicketsGroup> GetSummaryByLatest()
        {
            var tickets = GetLatestTickets();

            return tickets.GroupBy(t => $"{t.SerialName}-{t.ColorName} ({t.FirstNumber})").Select(g => new TicketsGroup
            {
                Name = g.Key,
                Count = g.Count(),
                HappyCount = g.Count(t => t.IsHappy)
            }).OrderByDescending(t => t.Count);
        }
    }
}
