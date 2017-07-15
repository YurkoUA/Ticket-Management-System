using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Data.EF.Enums
{
    public enum TaskPriority : byte
    {
        [Display(Name = "Дуже низький")]
        VeryLow = 1,

        [Display(Name = "Низький")]
        Low,

        [Display(Name = "Середній")]
        Medium,

        [Display(Name = "Високий")]
        High,

        [Display(Name = "Дуже високийю")]
        VeryHigh
    }
}
