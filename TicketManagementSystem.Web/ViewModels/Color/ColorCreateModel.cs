using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Web
{
    public class ColorCreateModel
    {
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Необхідно вказати назву.")]
        [StringLength(32, MinimumLength = 3, ErrorMessage = "Назва повинна бути від 3 до 32 символів.")]
        public string Name { get; set; }
    }
}