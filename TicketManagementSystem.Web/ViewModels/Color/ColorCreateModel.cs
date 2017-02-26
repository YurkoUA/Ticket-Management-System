using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TicketManagementSystem.Data.Interfaces;

namespace TicketManagementSystem.Web.ViewModels.Color
{
    public class ColorCreateModel : IRowVersion
    {
        [HiddenInput(DisplayValue = false)]
        public byte[] RowVersion { get; set; }

        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Необхідно вказати назву.")]
        [StringLength(32, MinimumLength = 3, ErrorMessage = "Назва повинна бути від 3 до 32 символів.")]
        public string Name { get; set; }
    }
}