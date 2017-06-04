using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Web
{
    public class LoginModel
    {
        [Display(Name = "Логін або Email")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати?")]
        public bool Remember { get; set; } = true;
    }
}