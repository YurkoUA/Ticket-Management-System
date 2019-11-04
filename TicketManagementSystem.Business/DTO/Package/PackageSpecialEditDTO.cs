using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using TicketManagementSystem.Data.Entities;

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
        
        [Obsolete]
        public double Nominal { get; set; }

        public int? NominalId { get; set; }

        [StringLength(128, ErrorMessage = "Довжина не може перевищувати 128 символів.")]
        public string Note { get; set; }

        public static Expression<Func<Package, PackageSpecialEditDTO>> CreateFromPackage = p => new PackageSpecialEditDTO
        {
            Id = p.Id,
            Name = p.Name,
            ColorId = p.ColorId,
            SerialId = p.SerialId,
            Nominal = p.Nominal,
            NominalId = p.NominalId,
            Note = p.Note
        };
    }
}
