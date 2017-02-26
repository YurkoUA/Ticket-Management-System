using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Web.ViewModels.Account
{
    public class ChangePasswordModel
    {
        [Display(Name = "Поточний пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Необхідно ввести поточний пароль.")]
        public string CurrentPassword { get; set; }

        [Display(Name = "Новий пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Необхідно ввести новий пароль.")]
        [RegularExpression("[A-Za-z0-9_]", ErrorMessage = "Можна використовувати лише символи (A-Z, a-z, 0-9, _).")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Пароль повинен бути від 8 до 64 символів.")]
        public string NewPassword { get; set; }

        [Display(Name = "Повторіть пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Необхідно підтвердити новий пароль.")]
        [Compare("NewPassword", ErrorMessage = "Паролі не збігаються.")]
        public string ConfirmedPassword { get; set; }
    }
}