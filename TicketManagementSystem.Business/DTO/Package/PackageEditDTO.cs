using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.DTO
{
    public class PackageEditDTO
    {
        public int Id { get; set; }

        public int ColorId { get; set; }
        public int SerialId { get; set; }

        [Range(0, 9, ErrorMessage = "Повинно бути число від 0 до 9.")]
        public int? FirstNumber { get; set; }

        [Required(ErrorMessage = "Необхідно вказати номінал пачки.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Номінал не може бути меншим за 0,1.")]
        public double Nominal { get; set; }

        [StringLength(128, ErrorMessage = "Довжина не може перевищувати 128 символів.")]
        public string Note { get; set; }

        public bool IsEmpty { get; set; }

        public static Expression<Func<Package, PackageEditDTO>> CreateFromPackage = p => new PackageEditDTO
        {
            Id = p.Id,
            ColorId = (int)p.ColorId,
            SerialId = (int)p.SerialId,
            FirstNumber = p.FirstNumber,
            Nominal = p.Nominal,
            Note = p.Note,
            IsEmpty = !p.Tickets.Any()
        };
    }
}
