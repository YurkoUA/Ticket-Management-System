using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using TicketManagementSystem.Business.Attributes;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.DTO
{
    public class TicketEditDTO
    {
        public int Id { get; set; }        
        public int ColorId { get; set; }        
        public int SerialId { get; set; }

        public bool CanSelectColor { get; set; } = true;
        public bool CanSelectSerial { get; set; } = true;

        [Required(ErrorMessage = "Необхідно вказати номер серії.")]
        [SerialNumber(ErrorMessage = "Номер серії повинен бути від 01 до 50.")]
        public string SerialNumber { get; set; }

        [StringLength(128, ErrorMessage = "Примітка не може бути більшою за 128 символів.")]
        public string Note { get; set; }

        [StringLength(32, ErrorMessage = "Дата не може бути довша за 32 символи.")]
        public string Date { get; set; }

        public static Expression<Func<Ticket, TicketEditDTO>> CreateFromTicket = t => new TicketEditDTO
        {
            Id = t.Id,
            ColorId = t.ColorId,
            SerialId = t.SerialId,
            SerialNumber = t.SerialNumber,
            Note = t.Note,
            Date = t.Date
        };
    }
}
