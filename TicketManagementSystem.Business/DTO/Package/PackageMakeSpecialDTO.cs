using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Business.DTO
{
    public class PackageMakeSpecialDTO
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Необхідно вказати назву пачки.")]
        [StringLength(64, MinimumLength = 2, ErrorMessage = "Назва може бути від 2 до 64 символів.")]
        public string Name { get; set; }

        [Display(Name = "Скинути колір")]
        public bool ResetColor { get; set; }

        [Display(Name = "Скинути серію")]
        public bool ResetSerial { get; set; }
    }
}
