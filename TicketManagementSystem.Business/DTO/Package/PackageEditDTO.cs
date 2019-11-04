using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Business.DTO
{
    public class PackageEditDTO
    {
        public int Id { get; set; }

        public int ColorId { get; set; }
        public int SerialId { get; set; }

        [Range(0, 9, ErrorMessage = "Повинно бути число від 0 до 9.")]
        public int? FirstNumber { get; set; }
        
        [Obsolete]
        public double Nominal { get; set; }

        public int NominalId { get; set; }

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
            NominalId = p.NominalId.Value,
            Note = p.Note,
            IsEmpty = !p.Tickets.Any()
        };
    }
}
