using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Data
{
    public enum UserRole
    {
        [Display(Name = "Користувач")]
        User,

        [Display(Name = "Адміністратор")]
        Admin
    }
}