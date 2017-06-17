using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class TicketChangeNumberModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Новий номер")]
        [Required(ErrorMessage = "Необхідно вказати номер квитка")]
        [RegularExpression(@"\d{6}", ErrorMessage = "Номер повинен складатися з шести цифр.")]
        public string Number { get; set; }
    }
}