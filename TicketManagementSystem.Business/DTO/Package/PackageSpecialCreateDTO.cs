using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Business.DTO
{
    public class PackageSpecialCreateDTO
    {
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
    }
}
