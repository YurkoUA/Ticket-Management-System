using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TicketManagementSystem.Business.Attributes;

namespace TicketManagementSystem.Web
{
    public class TicketChangeNumberModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Новий номер")]
        [Required(ErrorMessage = "Необхідно вказати номер квитка")]
        [TicketNumber(ErrorMessage = "Номер повинен складатися з шести цифр.")]
        public string Number { get; set; }
    }
}