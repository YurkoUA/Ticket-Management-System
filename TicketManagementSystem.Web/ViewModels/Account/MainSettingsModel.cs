using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Web
{
    public class MainSettingsModel
    {
        [Display(Name = "Ім'я:")]
        [StringLength(32, MinimumLength = 2, ErrorMessage = "Ім'я повинно бути від 2 до 32 символів.")]
        [Required(ErrorMessage = "Необхідно ввести ім'я.")]
        public string Name { get; set; }

        [Display(Name = "Двофакторна авторизація")]
        public bool TwoFactorAuthentication { get; set; }
    }
}