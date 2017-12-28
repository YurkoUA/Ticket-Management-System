﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            var tickets = Database.Tickets.GetAllIncluding(t => t.AddDate > date, t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets)
                .OrderBy(t => t.Number)
                .AsEnumerable();

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetTodayTickets(int timezoneOffset)
        {
            timezoneOffset *= -1;
            var today = DateTime.Today.ToUniversalTime();

            var tickets = Database.Tickets
                .GetAllIncluding(t => DbFunctions.TruncateTime(DbFunctions.AddMinutes(t.AddDate, timezoneOffset)) == today, 
                    t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets)
                .OrderBy(t => t.Number);

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
