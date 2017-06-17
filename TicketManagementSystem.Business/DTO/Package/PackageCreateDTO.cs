using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Business.DTO
{
    public class PackageCreateDTO
    {
        public int ColorId { get; set; }
        public int SerialId { get; set; }

        [Range(0, 9, ErrorMessage = "Повинно бути число від 0 до 9.")]
        public int? FirstNumber { get; set; }

        [Required(ErrorMessage = "Необхідно вказати номінал пачки.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Номінал не може бути меншим за 0,1.")]
        public double Nominal { get; set; }

        [StringLength(128, ErrorMessage = "Довжина не може перевищувати 128 символів.")]
        public string Note { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
