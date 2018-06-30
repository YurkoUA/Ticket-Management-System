using System;
using System.Linq.Expressions;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.DTO
{
    public class ColorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PackagesCount { get; set; }
        public int TicketsCount { get; set; }

        public bool CanBeDeleted => PackagesCount == 0 && TicketsCount == 0;

        public override string ToString() => Name;

        public static Expression<Func<Color, ColorDTO>> CreateFromColor = c => new ColorDTO
        {
            Id = c.Id,
            Name = c.Name,
            PackagesCount = c.Packages.Count,
            TicketsCount = c.Tickets.Count
        };
    }
}
