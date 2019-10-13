using System;
using System.Globalization;
using Newtonsoft.Json;
using TicketManagementSystem.Business.Extensions;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Business.DTO
{
    public class SummaryPeriodDTO
    {
        public SummaryPeriodDTO(Summary previous, Summary current)
        {
            Tickets = current.Tickets - previous.Tickets;
            HappyTickets = current.HappyTickets - previous.HappyTickets;
            Packages = current.Packages - previous.Packages;

            PeriodStartDate = previous.Date;
        }

        [JsonIgnore]
        public DateTime PeriodStartDate { get; set; }

        public string Period => GetMonthName(PeriodStartDate.Month).FirstCharToUpper() + " " + PeriodStartDate.Year;

        public int Tickets { get; set; }
        public int HappyTickets { get; set; }
        public int Packages { get; set; }

        public string GetMonthName(int monthNumber)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber);
        }
    }
}
