using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Data.EF
{
    public enum UserRole
    {
        [Display(Name = "Користувач")]
        User,

        [Display(Name = "Адміністратор")]
        Admin
    }
}