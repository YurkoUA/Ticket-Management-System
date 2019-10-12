using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.DTO
{
    public class PackageSpecialEditDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Необхідно вказати назву пачки.")]
        [StringLength(64, MinimumLength = 2, ErrorMessage = "Назва може бути від 2 до 64 символів.")]
        public string Name { get; set; }

        public int? ColorId { get; set; }
        public int? SerialId { get; set; }

        [Required(ErrorMessage = "Необхідно вказати номінал пачки.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Номінал не може бути меншим за 0,1.")]
        public double Nominal { get; set; }
        
        [StringLength(128, ErrorMessage = "Довжина не може перевищувати 128 символів.")]
        public string Note { get; set; }

        public static Expression<Func<Package, PackageSpecialEditDTO>> CreateFromPackage = p => new PackageSpecialEditDTO
        {
            Id = p.Id,
            Name = p.Name,
            ColorId = p.ColorId,
            SerialId = p.SerialId,
            Nominal = p.Nominal,
            Note = p.Note
        };
    }
}
