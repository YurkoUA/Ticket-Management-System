using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Web
{
    public class EntryConfirmationModel
    {
        [Display(Name = "Код підтвердження")]
        [Required(ErrorMessage = "Необхідно ввести код підтвердження.")]
        public string Code { get; set; }
    }
}