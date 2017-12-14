using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Business.Enums
{
    public enum PackageStatusFilter
    {
        [Display(Name = "(Всі)")]
        None,

        [Display(Name = "Звичайні")]
        Default,

        [Display(Name = "Спеціальні")]
        Special
    }
}
