using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Data.EF.Enums
{
    public enum TaskStatus : byte
    {
        [Display(Name = "Вільна")]
        None,

        [Display(Name = "В процесі")]
        InProgress,

        [Display(Name = "Перероблюється")]
        Recycle,

        [Display(Name = "Виконана")]
        Completed
    }
}
