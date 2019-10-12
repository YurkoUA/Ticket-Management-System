using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class ColorEditModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Назва")]
        [StringLength(32, MinimumLength = 3, ErrorMessage = "Назва повинна бути від 3 до 32 символів.")]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool CanBeDeleted { get; set; }
    }
}