using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Extensions;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business.Services
{
    public class StatisticsService : Service, IStatisticsService
    {
        private readonly ITicketService _ticketService;

        public StatisticsService(IUnitOfWork database, ITicketService ticketService) : base(database)
        {
            _ticketService = ticketService;
        }

        public IEnumerable<ChartDTO> TicketsBySerial()
        {
            return Database.Series.GetAll().Select(s => new ChartDTO
            {
                Name = s.Name,
                Count = s.Tickets.Count()
            }).OrderByDescending(s => s.Count);
        }

        public IEnumerable<ChartDTO> TicketsByColor()
        {
            return Database.Colours.GetAll().Select(c => new ChartDTO
            {
                Name = c.Name,
                Count = c.Tickets.Count()
            }).OrderByDescending(c => c.Count);
        }

        public IEnumerable<ChartDTO> TicketsByFirstNumber()
        {
            var groupsByNumber = Database.Tickets.GetAll()
                .GroupBy(t => SqlFunctions.Unicode(t.Number))
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    NumberUnicode = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return groupsByNumber.Select(g => new ChartDTO
            {
                Name = Convert.ToChar(g.NumberUnicode).ToString(),
                Count = g.Count
            });
        }

        public IEnumerable<ChartDTO> TicketsTypes()
        {
            var happy = _ticketService.CountHappyTickets();

            return new List<ChartDTO>
            {
                new ChartDTO { Name = "Звичайні", Count = _ticketService.TotalCount - happy },
                new ChartDTO { Name = "Щасливі", Count = happy }
            };
        }

        public IEnumerable<ChartDTO> HappyTicketsBySerial()
        {
            var tickets = Database.Tickets.GetAllIncluding(t => t.Serial)
                .Select(t => new { t.Number, Serial = t.Serial.Name })
                .ToList()
                .Where(t => t.Number.IsHappy());

            return tickets.GroupBy(t => t.Serial)
                .OrderByDescending(g => g.Count())
                .Select(g => new ChartDTO
                {
                    Name = g.Key,
                    Count = g.Count()
                });
        }

        public IEnumerable<ChartDTO> HappyTicketsByFirstNumber()
        {
            var numbers = Database.Tickets.GetAll()
                .Select(t => t.Number)
                .ToList()
                .Where(n => n.IsHappy());

            return numbers.GroupBy(n => n.First())
                .OrderBy(g => g.Key)
                .Select(g => new ChartDTO
                {
                    Name = g.Key.ToString(),
                    Count = g.Count()
                });
        }
    }
}
