using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Business.DTO
{
    public class SerialEditDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Необхідно вказати назву серії.")]
        [RegularExpression(@"[А-Я]{4}", ErrorMessage = "Назва серії може містити чотири великі літери українського алфавіту.")]
        public string Name { get; set; }

        [StringLength(128, ErrorMessage = "Довжина не може перевищувати 128 символів.")]
        public string Note { get; set; }

        public bool CanBeDeleted { get; set; }

        public static Expression<Func<Serial, SerialEditDTO>> CreateFromSerial = s => new SerialEditDTO
        {
            Id = s.Id,
            Name = s.Name,
            Note = s.Note,
            CanBeDeleted = s.Packages.Count == 0 && s.Tickets.Count == 0
        };
    }
}
