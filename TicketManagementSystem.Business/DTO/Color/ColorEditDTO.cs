using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.DTO
{
    public class ColorEditDTO
    {
        public int Id { get; set; }

        [StringLength(32, MinimumLength = 3, ErrorMessage = "Назва повинна бути від 3 до 32 символів.")]
        public string Name { get; set; }

        public bool CanBeDeleted { get; set; }
        public byte[] RowVersion { get; set; }

        public static Expression<Func<Color, ColorEditDTO>> CreateFromColor = c => new ColorEditDTO
        {
            Id = c.Id,
            Name = c.Name,
            RowVersion = c.RowVersion,
            CanBeDeleted = c.Packages.Count == 0 && c.Tickets.Count == 0
        };
    }
}
