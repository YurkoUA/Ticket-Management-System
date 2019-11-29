using System;
using System.Globalization;
using TicketManagementSystem.Infrastructure.Extensions;
using TicketManagementSystem.Infrastructure.Interfaces;
using TicketManagementSystem.ViewModels.Statistics;

namespace TicketManagementSystem.Domain.Statistics.Formatters
{
    public class TicketMonthSummaryFormatter : IStatisticsFormatter
    {
        public void Format(ChartDataVM chartData)
        {
            foreach (var item in chartData.Data)
            {
                var date = DateTime.Parse(item.Name);
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);

                item.Name = $"{monthName.FirstToUpper()} {date.Year}";
            }
        }
    }
}
