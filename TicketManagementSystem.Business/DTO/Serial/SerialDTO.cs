using System;
using System.Linq.Expressions;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Business.DTO
{
    public class SerialDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int PackagesCount { get; set; }
        public int TicketsCount { get; set; }

        public bool CanBeDeleted => PackagesCount == 0 && TicketsCount == 0;

        public override string ToString() => Name;

        public static Expression<Func<Serial, SerialDTO>> CreateFromSerial = s => new SerialDTO
        {
            Id = s.Id,
            Name = s.Name,
            Note = s.Note,
            PackagesCount = s.Packages.Count,
            TicketsCount = s.Tickets.Count
        };
    }
}
